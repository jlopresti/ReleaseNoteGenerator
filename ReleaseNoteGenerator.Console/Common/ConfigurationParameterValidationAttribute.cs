using System;
using System.Linq;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;
using ReleaseNoteGenerator.Console.SourceControl;

namespace ReleaseNoteGenerator.Console.Common
{
    public class ConfigurationParameterValidationAttribute : Attribute, IConfigurationValidationAttribute
    {
        private readonly string[] _parameters;

        public ConfigurationParameterValidationAttribute(params string[] parameters)
        {
            _parameters = parameters;
        }

        public void Validate(Expression<Func<JObject>> member)
        {
            var memberExp = member?.Body as MemberExpression;
            if (memberExp != null)
            {
                var value = member.Compile()();
                foreach (var parameter in _parameters)
                {
                    var jToken = value[parameter];
                    if (jToken == null || (!jToken.HasValues && string.IsNullOrEmpty(jToken.Value<string>())))
                        throw new ApplicationException($"{parameter} is missing in {memberExp.Member.Name} configuration");
                    if (jToken == null || (jToken.HasValues && !jToken.Values<string>().Any()))
                        throw new ApplicationException($"{parameter} is missing in {memberExp.Member.Name} configuration");
                }
            }
            else
            {
                throw new ArgumentException("member");
            }            
        }
    }
}