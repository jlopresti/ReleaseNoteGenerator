using System;
using Ranger.NetCore.Common;

namespace Ranger.NetCore.Console
{
    public interface IDependencyBootstrapper
    {
        IDependencyResolver Configure(Type application);
    }
}