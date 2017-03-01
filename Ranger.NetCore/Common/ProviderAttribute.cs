using System;

namespace Ranger.NetCore.Common
{
    public class ProviderAttribute : Attribute
    {
        public string Name { get; private set; }
        public ProviderAttribute(string name)
        {
            Name = name;
        }
    }
}