using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;
using Newtonsoft.Json;
using Ranger.Core.Common;
using Ranger.Core.IssueTracker;

namespace Ranger.Core.Helpers
{
    public static class Utils
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(DistinctIssue));
        public static T ToObject<T>(this string json)
        {
            if (string.IsNullOrEmpty(json)) return default(T);

            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (JsonException ex)
            {
                _logger.Debug("Invalid json", ex);
                return default(T);
            }   
        }

        public static List<ProviderAttribute> GetProviders<T>()
        {
            var type = typeof(T);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p))
                .Where(p => p.GetCustomAttribute<ProviderAttribute>() != null)
                .ToList();

            return types.Select(x => x.GetCustomAttribute<ProviderAttribute>()).ToList();
        }
    }
}
