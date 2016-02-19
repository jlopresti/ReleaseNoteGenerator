using System;
using System.IO;
using log4net;
using Newtonsoft.Json;

namespace ReleaseNoteGenerator.Console.Common
{
    internal class LocalPublisher : IPublisher
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(LocalPublisher));
        private LocalPublishConfig _config;

        public LocalPublisher(string configPath)
        {
            if (string.IsNullOrEmpty(configPath))
            {
                _logger.Error("Jira config must be provided", new NullReferenceException("configPath"));
                return;
            }
            if (!File.Exists(configPath))
            {
                _logger.Error("Jira config not found, please check config path", new FileNotFoundException($"{configPath} doesn't exist."));
                return;
            }
            _config = File.ReadAllText(configPath).ToObject<LocalPublishConfig>();
            if (_config == null)
            {
                _logger.Error("Invalid jira config", new JsonException("Json is invalid"));
                return;
            }
        }

        public bool Publish(string output)
        {
            var directory = Path.GetDirectoryName(_config.OutputFile);
            if (string.IsNullOrEmpty(directory) || !Directory.Exists(directory))
            {
                _logger.Error($"{_config.OutputFile} doesn't exist.");
                return false;
            }

            File.WriteAllText(_config.OutputFile, output);
            return true;
        }
    }
}