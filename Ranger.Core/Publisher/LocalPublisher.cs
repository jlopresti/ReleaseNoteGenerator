using System.IO;
using log4net;
using Newtonsoft.Json.Linq;
using Ranger.Core.Common;
using Ranger.Core.Helpers;
using Ranger.Core.Models.Publisher;

namespace Ranger.Core.Publisher
{
    [Provider("local")]
    [ConfigurationParameterValidation("outputfile")]
    internal class LocalPublisher : IPublisher
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(LocalPublisher));
        private LocalPublishConfig _config;

        public LocalPublisher(JObject configPath)
        {
            _config = configPath.ToObject<LocalPublishConfig>();
            Guard.IsNotNull(() => _config);
        }

        public bool Publish(string releaseNumber, string output)
        {
            File.WriteAllText(_config.OutputFile, output);
            return true;
        }
    }
}