using System;

namespace MapReduce.Net
{
    public interface IDependancyScope : IDisposable
    {
        T Resolve<T>();
        object Resolve(Type t);
    }
}
