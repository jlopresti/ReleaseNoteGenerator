using System.Collections.Generic;

namespace Ranger.NetCore.Console
{
    public interface IDependencyResolver
    {
        T Resolve<T>() where T : class;
        IEnumerable<T> ResolveAll<T>() where T : class;
    }
}