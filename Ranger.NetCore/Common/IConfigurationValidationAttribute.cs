using System;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;

namespace Ranger.NetCore.Common
{
    public interface IConfigurationValidationAttribute
    {
        void Validate(Expression<Func<JObject>> member);
    }
}