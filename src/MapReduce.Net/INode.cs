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
        Task<List<KeyValuePair<TMapperKey, TMapperValue>>> RunTasks<TMapperKey, TMapperValue>();
        ICombiner Combiner { get; }
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

        
        public ICombiner Combiner { get; set; }
        public async Task<List<KeyValuePair<TMapperKey, TMapperValue>>> RunTasks<TMapperKey, TMapperValue>()
        {
            await Task.WhenAll(MapperTasks);

            // concat all keyvalue pairs
            var allKeyValuePairsForNode = new List<List<KeyValuePair<TMapperKey, TMapperValue>>>();
            for (int i = 0; i < Mappers.Count; i++)
            {
                var current = (List<KeyValuePair<TMapperKey, TMapperValue>>)Mappers[i].GetType().GetRuntimeProperty("KeyValuePairs").GetValue(Mappers[i], null);
                allKeyValuePairsForNode.Add(current);
            }

            var flattenList = allKeyValuePairsForNode.SelectMany(x => x.ToList());
            if (_configurator.TypeOfCombiner != null)
            {
                var combiner = (ICombiner)Activator.CreateInstance(_configurator.TypeOfCombiner);
                var combineMethod = _configurator.TypeOfCombiner.GetRuntimeMethods().Single(m => m.Name == "Combine" && m.IsPublic && m.GetParameters().Any());


                var combineTask = (Task<List<KeyValuePair<TMapperKey, TMapperValue>>>)combineMethod.Invoke(combiner, new object[] { combiner.GetHashCode().ToString(), flattenList });
                return combineTask.Result;
            }
            return flattenList.ToList();
        }
    }
}
