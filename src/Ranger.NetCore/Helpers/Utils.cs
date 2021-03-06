﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Ranger.NetCore.Common;

namespace Ranger.NetCore.Helpers
{
    public static class Utils
    {
        public static T ToObject<T>(this string json)
        {
            if (string.IsNullOrEmpty(json)) return default(T);

            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (JsonException ex)
            {
                return default(T);
            }
        }

        public static T GetPlugins<T>(this IEnumerable<T> source, string plugin)
            where T : IRangerPlugin
        {
            return source.SingleOrDefault(x => x.PluginId.Equals(plugin, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
