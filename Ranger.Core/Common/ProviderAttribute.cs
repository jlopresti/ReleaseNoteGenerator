using System;

namespace Ranger.Core.Common
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