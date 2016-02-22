using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReleaseNoteGenerator.Console.Exceptions
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
