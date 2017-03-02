using System;

namespace Ranger.NetCore.Console
{
    public interface IDependencyBootstrapper
    {
        IDependencyResolver Configure(Type application);
    }
}