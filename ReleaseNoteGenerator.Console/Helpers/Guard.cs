using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;
using Ranger.Console.Common;
using Ranger.Console.Models;

namespace Ranger.Console.Helpers
{
    public static class Guard
    {
        public static void IsNotNullOrEmpty(Expression<Func<string>> member)
        {
            var memberExp = member?.Body as MemberExpression;
            if (memberExp != null)
            {
                var value = member.Compile()();
                if (string.IsNullOrEmpty(value))
                    throw new ApplicationException($"{memberExp.Member.Name} is missing.");
            }
            else
            {
                throw new ArgumentException("member");
            }

        }

        public static void IsNotNull<T>(Expression<Func<T>> member)
        {
            var memberExp = member?.Body as MemberExpression;
            if (memberExp != null)
            {
                var value = member.Compile()();
                if (value == null)
                    throw new ApplicationException($"{memberExp.Member.Name} is missing.");
            }

            else
            {
                throw new ArgumentException("member");
            }
        }

        public static void IsNotNull(params Expression<Func<object>>[] values)
        {
            foreach (var value in values)
            {
                Guard.IsNotNull<object>(value);
            }
        }

        public static void IsValidFilePath(Expression<Func<string>> member)
        {
            var memberExp = member?.Body as MemberExpression;
            if (memberExp != null)
            {
                var value = member.Compile()();
                Guard.IsNotNullOrEmpty(member);
                if (!File.Exists(value))
                    throw new ApplicationException($"{memberExp.Member.Name} is not a valid path : {value}.");
            }
            else
            {
                throw new ArgumentException("member");
            }
        }

        public static void IsValidConfig(Expression<Func<Config>> member)
        {
            var memberExp = member?.Body as MemberExpression;
            if (memberExp != null)
            {
                var value = member.Compile()();
                if(value == null)
                    throw new ApplicationException("Configuration file is invalid. Please check it.");
                if (value.SourceControl == null)
                    throw new ApplicationException($"No source control configuration set.");
                if (value.IssueTracker == null)
                    throw new ApplicationException($"No issue tracker configuration set.");
                if (value.Template == null)
                    throw new ApplicationException($"No template configuration set.");
                if (value.Publish == null)
                    throw new ApplicationException($"No publish configuration set.");
            }
            else
            {
                throw new ArgumentException("member");
            }
        }

        public static void ProviderRequired(Expression<Func<JObject>> member)
        {
            var memberExp = member?.Body as MemberExpression;
            if (memberExp != null)
            {
                var value = member.Compile()();
                if (string.IsNullOrEmpty(value.GetProvider()))
                    throw new ApplicationException($"{memberExp.Member.Name} provider is missing in configuration file.");
            }
            else
            {
                throw new ArgumentException("member");
            }
        }

        public static void ValidateConfigParameter(Type provider, Expression<Func<JObject>> member)
        {
            var validators = provider.GetCustomAttributes(typeof (IConfigurationValidationAttribute), true)
                    .Cast<IConfigurationValidationAttribute>()
                    .ToList();

            foreach (var validator in validators)
            {
                validator.Validate(member);
            }
        }
    }
}