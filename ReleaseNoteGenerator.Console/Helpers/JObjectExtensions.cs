using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;
using ReleaseNoteGenerator.Console.Common;
using ReleaseNoteGenerator.Console.SourceControl;

namespace ReleaseNoteGenerator.Console.Helpers
{
    public static class JObjectExtensions
    {
        public static string GetProvider(this JObject obj)
        {
            JToken token;
            if (obj.TryGetValue("provider", out token))
            {
                return token.ToString();
            }
            return null;
        }
        public static string GetCommitMessagePattern(this JObject obj)
        {
            JToken token;
            if (obj.TryGetValue("messageCommitPattern", out token))
            {
                return token.ToString();
            }
            return null;
        }

        public static Type GetTypeProvider<T>(this JObject obj)
        {
            var provider = obj.GetProvider();
            var type = typeof(T);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p))
                .Where(p => p.GetCustomAttributes(typeof(ProviderAttribute)).Cast<ProviderAttribute>().Any(x => x.Name.Equals(provider, StringComparison.InvariantCultureIgnoreCase)))
                .ToList();
            return types.FirstOrDefault();
        }
    }
}