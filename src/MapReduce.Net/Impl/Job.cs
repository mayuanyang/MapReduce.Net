using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MapReduce.Net.Impl
{
    public class Job : IJob
    {
        private readonly IJobConfigurator _configurator;
        private readonly List<Node> _nodes;

        public Job(IJobConfigurator configurator)
        {
            _configurator = configurator;
            _nodes = new List<Node>();

        }
        public async Task<TReturnData> Run<TInputData, TReturnData, TMapperKeyIn, TMapperValueIn, TMapperOutputKey, TMapperOutputValue>(TInputData inputData)
        {
            _configurator.ValidateConfiguration();

            var numOfMappersPerNode = _configurator.NumberOfMappersPerNode;
            if (numOfMappersPerNode == 0)
            {
                numOfMappersPerNode = Environment.ProcessorCount;
            }

            IDataBatchProcessor<TInputData, List<TInputData>> dataProcessor;
            if (_configurator.DependancyScope == null)
            {
                dataProcessor = (IDataBatchProcessor<TInputData, List<TInputData>>)Activator.CreateInstance(_configurator.TypeOfDataBatchProcessor);
            }
            else
            {
                dataProcessor = (IDataBatchProcessor<TInputData, List<TInputData>>)_configurator.DependancyScope.Resolve(_configurator.TypeOfDataBatchProcessor);                
            }
            var chunks = await dataProcessor.Run(inputData);
            var reduceResult = await InternalRun<TInputData, TReturnData, TMapperKeyIn, TMapperValueIn, TMapperOutputKey, TMapperOutputValue>(chunks, numOfMappersPerNode);
            return reduceResult;
        }

        private async Task<TReturnData> InternalRun<TInputData, TReturnData, TMapperKeyIn, TMapperValueIn, TMapperOutputKey, TMapperOutputValue>(IList chunks, int numOfMappersPerNode)
        {
            int numOfNodes = 1;
            if (numOfMappersPerNode > chunks.Count)
            {
                numOfMappersPerNode = chunks.Count;
            }
            else
            {
                numOfNodes = (int)decimal.Ceiling(chunks.Count / (decimal)numOfMappersPerNode);
            }

            for (int i = 0; i < numOfNodes; i++)
            {
                var n = new Node(i.ToString(), _configurator);
                _nodes.Add(n);
            }

            int nodeIndex = 0;
            int chunkIndex = 0;
            var nodeTasks = new List<Task<List<KeyValuePair<TMapperOutputKey, TMapperOutputValue>>>>();
            foreach (var item in chunks)
            {
                chunkIndex += 1;
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
                Task mapTask;
                IMapper<TMapperKeyIn, TMapperValueIn, TMapperOutputKey, TMapperOutputValue> mapper;
                if (_configurator.DependancyScope == null)
                {
                    mapper = (IMapper<TMapperKeyIn, TMapperValueIn, TMapperOutputKey, TMapperOutputValue>)Activator.CreateInstance(_configurator.TypeOfMapper);
                }
                else
                {
                    mapper = (IMapper<TMapperKeyIn, TMapperValueIn, TMapperOutputKey, TMapperOutputValue>)_configurator.DependancyScope.Resolve(_configurator.TypeOfMapper);
                }
                mapTask = Task.Run(() => mapper.Map((TMapperKeyIn)(object)mapper.GetHashCode().ToString(), (TMapperValueIn)item));
                node.MapperTasks.Add(mapTask);
                node.Mappers.Add(mapper);


                if (node.Mappers.Count == numOfMappersPerNode)
                {
                    // Run task as soon as the Node is ready
                    nodeTasks.Add(Task.Run(() => node.RunTasks<TMapperOutputKey, TMapperOutputValue>()));
                }
                else
                {
                    if (chunkIndex == chunks.Count)
                    {
                        nodeTasks.Add(Task.Run(() => node.RunTasks<TMapperOutputKey, TMapperOutputValue>()));
                    }
                }

            }

            if (nodeTasks.Count != numOfNodes)
            {
                throw new Exception("Inbalanced node tasks and nodes");
            }

            await Task.WhenAll(nodeTasks);

            var allKeyValuePairsFromNodes = nodeTasks.Select(t => t.Result).ToList();

            // Run reducer
            object reducer;
            var flattenList = allKeyValuePairsFromNodes.SelectMany(x => x.ToList()).ToList();
            reducer = _configurator.DependancyScope == null ? Activator.CreateInstance(_configurator.TypeOfReducer) : _configurator.DependancyScope.Resolve(_configurator.TypeOfReducer);
            var reduceMethod = _configurator.TypeOfReducer.GetRuntimeMethods().Single(m => m.Name == "Reduce" && m.IsPublic && m.GetParameters().Any());
            var reduceResult = await (Task<TReturnData>)reduceMethod.Invoke(reducer, new object[] { reducer.GetHashCode().ToString(), flattenList });
            return reduceResult;

        }
    }
}