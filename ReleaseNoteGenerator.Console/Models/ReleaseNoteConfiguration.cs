using System.IO;
using log4net;
using Ranger.Console.Common;
using Ranger.Console.Helpers;

namespace Ranger.Console.Models
{
    public class ReleaseNoteConfiguration : BaseConfiguration<Settings>
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(ReleaseNoteConfiguration));

        public Config Config { get; private set; }       
        public string ReleaseNumber { get; private set; }
        protected override bool ValidateConfig(string[] args)
        {
            Guard.IsValidFilePath(() => Settings.Value.ConfigPath);
            ReleaseNumber = Settings.Value.ReleaseNumber;

            _logger.DebugFormat("[APP] Reading config file at {0}", Settings.Value.ConfigPath);
            Config = File.ReadAllText(Settings.Value.ConfigPath).ToObject<Config>();

            Guard.IsValidConfig(() => Config);
            return true;
        }
    }
}