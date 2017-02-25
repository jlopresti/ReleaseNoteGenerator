using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;
using Newtonsoft.Json;
using Ranger.NetCore.Common;
using Ranger.NetCore.IssueTracker;

namespace Ranger.NetCore.Helpers
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
            //TODO : Don't forget to implement Plugin system
            throw new NotImplementedException();
            //var type = typeof(T);
            //var types = Assembly.GetEntryAssembly()?.GetReferencedAssemblies()
            //    .Select(s => s.GetType())
            //    .Where(p => type.IsAssignableFrom(p))
            //    .Where(p => p.GetCustomAttribute<ProviderAttribute>() != null)
            //    .ToList();

            //return types.Select(x => x.GetCustomAttribute<ProviderAttribute>()).ToList();
        }
    }
}
