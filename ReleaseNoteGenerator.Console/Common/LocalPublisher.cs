﻿using System;
using System.IO;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ReleaseNoteGenerator.Console.Common
{
    internal class LocalPublisher : IPublisher
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(LocalPublisher));
        private LocalPublishConfig _config;

        public LocalPublisher(JObject configPath)
        {
            _config = configPath.ToObject<LocalPublishConfig>();
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