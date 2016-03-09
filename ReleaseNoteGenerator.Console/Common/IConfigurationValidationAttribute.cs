using System;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;

namespace Ranger.Console.Common
{
    public interface IConfigurationValidationAttribute
    {
        void Validate(Expression<Func<JObject>> member);
    }
}