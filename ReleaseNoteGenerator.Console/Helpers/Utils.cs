using log4net;
using Newtonsoft.Json;
using Ranger.Console.IssueTracker;

namespace Ranger.Console.Helpers
{
    public static class JsonExtentensions
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
    }
}
