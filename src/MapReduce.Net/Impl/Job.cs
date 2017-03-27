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
        public async Task Run(TData inputData)
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
                var chunks = (IEnumerable)resultProperty.Invoke(prepareDataTask, new object[] { });

                
                var context = new ExecutionContext(new List<IMapper>(), new List<IReducer>());
                var mapperTasks = new List<Task>();
                foreach (var item in chunks)
                {
                    // Create one mapper for each chunk and start mapping
                    var mapper = Activator.CreateInstance(_configurator.TypeOfMapper);
                    var mapMethod = _configurator.TypeOfMapper.GetRuntimeMethods().Single(m => m.Name == "Map" && m.IsPublic && m.GetParameters().Any());
                    var mapTask = Task.Run(() => mapMethod.Invoke(mapper, new[] {mapper.GetHashCode().ToString(), item }));
                    mapperTasks.Add(mapTask);
                    context.Mappers.Add(mapper as IMapper);
                }
                await Task.WhenAll(mapperTasks);

                // Use partitioner to shuffle the data to each reducer
                foreach (IMapper mapper in context.Mappers)
                {
                    // union all keyvalue pairs
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