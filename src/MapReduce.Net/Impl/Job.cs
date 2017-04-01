using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MapReduce.Net.Impl
{
    public class Job<TInputData, TReturnData> : IJob<TInputData, TReturnData>
    {
        private readonly IJobConfigurator _configurator;
        private readonly List<Node> _nodes;

        public Job(IJobConfigurator configurator)
        {
            _configurator = configurator;
            _nodes = new List<Node>();

        }
        public async Task<TReturnData> Run<TMapperOutputKey, TMapperOutputValue>(TInputData inputData)
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
                var numOfMappersPerNode = _configurator.NumberOfMappersPerNode;

                if (numOfMappersPerNode == 0)
                {
                    numOfMappersPerNode = Environment.ProcessorCount * 20;
                }

                if (numOfMappersPerNode > chunks.Count)
                {
                    numOfMappersPerNode = chunks.Count;
                }
                
                int numOfNodes = (int)decimal.Ceiling(chunks.Count / (decimal)numOfMappersPerNode);
                if (numOfNodes == 0) numOfNodes = 1;
                for (int i = 0; i < numOfNodes; i++)
                {
                    var n = new Node(i.ToString(), _configurator);
                    _nodes.Add(n);
                }

                int nodeIndex = 0;
                var nodeTasks = new List<Task<List<KeyValuePair<TMapperOutputKey, TMapperOutputValue>>>>();
                foreach (var item in chunks)
                {

                    var node = _nodes[nodeIndex];
                    if (node.Mappers.Count == numOfMappersPerNode)
                    {
                        nodeIndex += 1;
                        if (nodeIndex < _nodes.Count)
                        {
                            node = _nodes[nodeIndex];
                        }
                    }
                    // Create one mapper for each chunk and start mapping
                    var mapper = Activator.CreateInstance(_configurator.TypeOfMapper);
                    var mapMethod = _configurator.TypeOfMapper.GetRuntimeMethods()
                        .Single(m => m.Name == "Map" && m.IsPublic && m.GetParameters().Any());
                    var mapTask = Task.Run(() => mapMethod.Invoke(mapper, new[] { mapper.GetHashCode().ToString(), item }));
                    node.MapperTasks.Add(mapTask);
                    node.Mappers.Add(mapper as IMapper);
                    if (node.Mappers.Count == numOfMappersPerNode)
                    {
                        // Run task as soon as the Node is ready
                        nodeTasks.Add(Task.Run(() => node.RunTasks<TMapperOutputKey, TMapperOutputValue>()));
                    }
                }
                await Task.WhenAll(nodeTasks);

                var allKeyValuePairsFromNodes = new List<List<KeyValuePair<TMapperOutputKey, TMapperOutputValue>>>();
                foreach (var t in nodeTasks)
                {
                    allKeyValuePairsFromNodes.Add(t.Result);
                }

                // Run reducer
                var reducer = Activator.CreateInstance(_configurator.TypeOfReducer);
                var reduceMethod = _configurator.TypeOfReducer.GetRuntimeMethods().Single(m => m.Name == "Reduce" && m.IsPublic && m.GetParameters().Any());

                var flattenList = allKeyValuePairsFromNodes.SelectMany(x => x.ToList()).ToList();


                var reduceTask = await (Task<TReturnData>)reduceMethod.Invoke(reducer, new object[] { reducer.GetHashCode().ToString(), flattenList });
                
                return reduceTask;


            }
            else
            {
                throw new NotImplementedException();

            }
        }

    }
}