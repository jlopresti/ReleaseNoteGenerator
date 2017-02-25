using System.IO;
using log4net;
using Ranger.NetCore.Common;
using Ranger.NetCore.Helpers;
using Ranger.NetCore.Models.publisher;

namespace Ranger.NetCore.Publisher
{
    [Provider("local", ConfigurationType = typeof(LocalPublishConfig))]
    [ConfigurationParameterValidation("outputfile")]
    internal class LocalPublisher : IPublisher
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(LocalPublisher));
        private LocalPublishConfig _config;

        public LocalPublisher(LocalPublishConfig config)
        {
            _config = config;
            Guard.IsNotNull(() => _config);
        }

        public bool Publish(string releaseNumber, string output)
        {
            File.WriteAllText(_config.OutputFile, output);
            return true;
        }
    }
}