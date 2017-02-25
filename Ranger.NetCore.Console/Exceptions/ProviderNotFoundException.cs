using System;

namespace Ranger.NetCore.Console.Exceptions
{
    public class ProviderNotFoundException : Exception
    {
        private readonly string _provider;

        public ProviderNotFoundException(string provider) : base(
            $"No provider found with id : {provider}")
        {
            _provider = provider;
        }
    }
}
