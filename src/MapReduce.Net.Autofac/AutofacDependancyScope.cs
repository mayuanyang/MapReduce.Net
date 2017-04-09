using System;
using Autofac;

namespace MapReduce.Net.Autofac
{
    public class AutofacDependancyScope : IDependancyScope
    {
        private readonly ILifetimeScope _lifetimeScope;

        public AutofacDependancyScope(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        public void Dispose()
        {
            _lifetimeScope.Dispose();
        }

        public T Resolve<T>()
        {
            return _lifetimeScope.Resolve<T>();
        }

        public object Resolve(Type t)
        {
            return _lifetimeScope.Resolve(t);
        }
    }
}
