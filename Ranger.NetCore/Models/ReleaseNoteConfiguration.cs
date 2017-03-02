using System;
using System.IO;
using log4net;
using Ranger.NetCore.Helpers;

namespace Ranger.NetCore.Models
{
    public class ReleaseNoteConfiguration : IReleaseNoteConfiguration
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(ReleaseNoteConfiguration));
        private bool _isConfigLoaded = false;
        private Config _config;
        public string ReleaseNumber { get; private set; }

        public T GetSourceControlConfig<T>()
        {
            if (!_isConfigLoaded)
            {
                throw new InvalidOperationException("LoadConfigFile should be called before");
            }

            return _config.SourceControl.ToObject<T>();
        }

        public T GetIssueTrackerConfig<T>()
        {
            if (!_isConfigLoaded)
            {
                throw new InvalidOperationException("LoadConfigFile should be called before");
            }

            return _config.IssueTracker.ToObject<T>();
        }

        public T GetPublisherConfig<T>()
        {
            if (!_isConfigLoaded)
            {
                throw new InvalidOperationException("LoadConfigFile should be called before");
            }

            return _config.Publish.ToObject<T>();
        }

        public T GetTemplateConfig<T>()
        {
            if (!_isConfigLoaded)
            {
                throw new InvalidOperationException("LoadConfigFile should be called before");
            }

            return _config.Template.ToObject<T>();
        }

        public bool LoadConfigFile(string path, string release)
        {
            //Guard.IsValidFilePath(() => path);
            ReleaseNumber = release;
            _logger.DebugFormat("[APP] Reading config file at {0}", path);
            _config = File.ReadAllText(path).ToObject<Config>();            
            _isConfigLoaded = true;

            return true;
        }
    }
}