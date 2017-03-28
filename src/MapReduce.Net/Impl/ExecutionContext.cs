using System.Collections.Generic;

namespace MapReduce.Net.Impl
{
    public sealed class ExecutionContext
    {
        internal IList<INode> Nodes { get; }
        
        public ExecutionContext()
        {
            Nodes = new List<INode>();
        }
    }
}
