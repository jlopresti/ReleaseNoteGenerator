using System;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;

namespace Ranger.Core.Common
{
    public interface IConfigurationValidationAttribute
    {
        void Validate(Expression<Func<JObject>> member);
    }
}