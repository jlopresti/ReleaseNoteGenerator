using System.IO;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReleaseNoteGenerator.Console.Common;
using ReleaseNoteGenerator.Console.Helpers;
using ReleaseNoteGenerator.Console.Models.Publisher;
using ReleaseNoteGenerator.Console.SourceControl;

namespace ReleaseNoteGenerator.Console.Publlsher
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