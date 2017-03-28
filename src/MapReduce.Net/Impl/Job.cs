using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MapReduce.Net.Impl
{
    public class Job<TData> : IJob<TData>
    {
        private readonly IJobConfigurator _configurator;

        public Job(IJobConfigurator configurator)
        {
            _configurator = configurator;
        }
        public async Task Run<TKey, TValue>(TData inputData)
        {
            if (_configurator.TypeOfMapper == null)
            {
                throw new ArgumentException("Type of mapper is not provided");
            }
            if (_configurator.TypeOfReducer == null)
            {
                throw new ArgumentException("Type of reducer is not provided");
            }
            if (_configurator.TypeOfDataBatchProcessor == null)
            {
                throw new ArgumentException("Type of IDataBatchProcessor is not provided");
            }

            if (_configurator.DependancyScope == null)
            {
                var dataProcessor = Activator.CreateInstance(_configurator.TypeOfDataBatchProcessor);
                
                // Prepare the data as ienumerable
                var runMethodFromDataProcessor = _configurator.TypeOfDataBatchProcessor.GetRuntimeMethods().Single(m => m.Name == "Run" && m.IsPublic && m.GetParameters().Any());
                var prepareDataTask = (Task)runMethodFromDataProcessor.Invoke(dataProcessor, new object[] { inputData });
                var resultProperty = prepareDataTask.GetType().GetTypeInfo().GetDeclaredProperty("Result").GetMethod;
                var chunks = (IList)resultProperty.Invoke(prepareDataTask, new object[] { });

                var context = new ExecutionContext();
                var mapperTasks = new List<Task>();
                
                if (chunks.Count >= 2)
                {
                    int numOfNodes = (int)decimal.Ceiling(chunks.Count / (decimal)_configurator.NumberOfMappersPerNode);
                    for (int i = 0; i < numOfNodes; i++)
                    {
                        var n = new Node();
                        context.Nodes.Add(n);
                    }

                    int nodeIndex = 0;
                    foreach (var item in chunks)
                    {
                        // Add mapper into node
                        var node = context.Nodes[nodeIndex];
                        if (node.Mappers.Count == _configurator.NumberOfMappersPerNode)
                        {
                            nodeIndex += 1;
                            if (nodeIndex < context.Nodes.Count)
                            {
                                node = context.Nodes[nodeIndex];
                            }
                        }
                        
                        // Create one mapper for each chunk and start mapping
                        var mapper = Activator.CreateInstance(_configurator.TypeOfMapper);
                        var mapMethod = _configurator.TypeOfMapper.GetRuntimeMethods().Single(m => m.Name == "Map" && m.IsPublic && m.GetParameters().Any());
                        var mapTask = Task.Run(() => mapMethod.Invoke(mapper, new[] { mapper.GetHashCode().ToString(), item }));
                        mapperTasks.Add(mapTask);
                        node.Mappers.Add(mapper as IMapper);
                    }
                }
                
                await Task.WhenAll(mapperTasks);

                var combineTasks = new List<Task>();
                foreach (INode node in context.Nodes)
                {
                    // union all keyvalue pairs
                    if (_configurator.TypeOfCombiner != null)
                    {
                        var allObjects = (List<KeyValuePair<TKey, TValue>>)node.Mappers[0].GetType().GetRuntimeProperty("KeyValuePairs").GetValue(node.Mappers[0], null);
                        for (int i = 1; i< node.Mappers.Count; i++)
                        {
                            var current = (List<KeyValuePair<TKey, TValue>>)node.Mappers[i].GetType().GetRuntimeProperty("KeyValuePairs").GetValue(node.Mappers[i], null);
                            allObjects = allObjects.Concat(current).ToList();
                        }
                        var combiner = (ICombiner)Activator.CreateInstance(_configurator.TypeOfCombiner);
                        node.Combiner = combiner;
                        var combineMethod = _configurator.TypeOfCombiner.GetRuntimeMethods().Single(m => m.Name == "Combine" && m.IsPublic && m.GetParameters().Any());
                        var combineTask = Task.Run(() => combineMethod.Invoke(combiner, new object[] { combiner.GetHashCode().ToString(), allObjects}));
                        combineTasks.Add(combineTask);
                    }
                }
                if (combineTasks.Count > 0)
                {
                    await Task.WhenAll(combineTasks);
                }

                // Run reduce in each reducer

            }
            else
            {
                throw new NotImplementedException();
                
            }
         
        }
    }
}