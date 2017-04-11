using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MapReduce.Net
{
    interface INode
    {
        IList<IMapper> Mappers { get; }
        IList<Task> MapperTasks { get; }
        Task<List<KeyValuePair<TMapperOutputKey, TMapperOutputValue>>> RunTasks<TMapperOutputKey, TMapperOutputValue>();
        IReducer Combiner { get; }
    }

    class Node : INode
    {
        private readonly IJobConfigurator _configurator;
        public string Name { get; }

        public Node(string name, IJobConfigurator _configurator)
        {
            this._configurator = _configurator;
            Name = name;
            Mappers = new List<IMapper>();
            MapperTasks = new List<Task>();

        }
        public IList<IMapper> Mappers { get; }
        public IList<Task> MapperTasks { get; }

        
        public IReducer Combiner { get; set; }
        public async Task<List<KeyValuePair<TMapperOutputKey, TMapperOutputValue>>> RunTasks<TMapperOutputKey, TMapperOutputValue>()
        {
            await Task.WhenAll(MapperTasks);

            // concat all keyvalue pairs
            var allKeyValuePairsForNode = new List<List<KeyValuePair<TMapperOutputKey, TMapperOutputValue>>>();
            foreach (var mapperTask in MapperTasks)
            {
                var resultProperty = mapperTask.GetType().GetRuntimeProperty("Result").GetMethod;
                var result = (List<KeyValuePair<TMapperOutputKey, TMapperOutputValue>>)resultProperty.Invoke(mapperTask, new object[] { });
                allKeyValuePairsForNode.Add(result);
            }
            var flatternList = allKeyValuePairsForNode.SelectMany(x => x.ToList()).ToList();
            if (_configurator.TypeOfCombiner != null)
            {
                var combiner = (IReducer)Activator.CreateInstance(_configurator.TypeOfCombiner);
                var combineMethod = _configurator.TypeOfCombiner.GetRuntimeMethods().Single(m => m.Name == "Combine" && m.IsPublic && m.GetParameters().Any());
                
                var combineTask = (Task<List<KeyValuePair<TMapperOutputKey, TMapperOutputValue>>>)combineMethod.Invoke(combiner, new object[] { combiner.GetHashCode().ToString(), flatternList });
                return combineTask.Result;
            }
           
            return flatternList;
        }
    }
}
