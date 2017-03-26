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

        public Job(IJobConfigurator configurator)
        {
            _configurator = configurator;
        }
        public async Task Run(object inputData)
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
                
                var runMethodFromDataProcessor = _configurator.TypeOfDataBatchProcessor.GetRuntimeMethods().Single(m => m.Name == "Run" && m.IsPublic && m.GetParameters().Any());
                var prepareDataTask = (Task)runMethodFromDataProcessor.Invoke(dataProcessor, new object[] { inputData });
                var resultProperty = prepareDataTask.GetType().GetTypeInfo().GetDeclaredProperty("Result").GetMethod;

                var chunks = (IEnumerable)resultProperty.Invoke(prepareDataTask, new object[] { });
                var context = Activator.CreateInstance(_configurator.TypeOfContext);
                var mapperTasks = new List<Task>();
                foreach (var item in chunks)
                {
                    var mapper = Activator.CreateInstance(_configurator.TypeOfMapper);
                    var mapMethod = _configurator.TypeOfMapper.GetRuntimeMethods().Single(m => m.Name == "Map" && m.IsPublic && m.GetParameters().Any());
                    var mapTask = Task.Run(() => mapMethod.Invoke(mapper, new object[] {mapper.GetHashCode().ToString(), item, context }));
                    mapperTasks.Add(mapTask);
                }
                await Task.WhenAll(mapperTasks);
            }
            else
            {
                throw new NotImplementedException();
                
            }
         
        }
    }
}