using System.Collections.Generic;

namespace MapReduce.Net
{
    interface INode
    {
        IList<IMapper> Mappers { get; }
        ICombiner Combiner { get; set; }
    }

    class Node : INode
    {
        public Node()
        {
            Mappers = new List<IMapper>();
        }
        public IList<IMapper> Mappers { get; }
        public ICombiner Combiner { get; set; }
    }
}
