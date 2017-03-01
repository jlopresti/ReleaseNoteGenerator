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
        private static readonly ILog _logger = LogManager.GetLogger(typeof(Utils));
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
    }
}
