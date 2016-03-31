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
        public bool StartWebServer { get; private set; }

        protected override bool ValidateConfig(string[] args)
        {

            if (InvokedVerb == "generate")
            {
                Guard.IsValidFilePath(() => Settings.Value.GenerateVerb.ConfigPath);
                ReleaseNumber = Settings.Value.GenerateVerb.ReleaseNumber;
                _logger.DebugFormat("[APP] Reading config file at {0}", Settings.Value.GenerateVerb.ConfigPath);
                Config = File.ReadAllText(Settings.Value.GenerateVerb.ConfigPath).ToObject<Config>();
                Guard.IsValidConfig(() => Config);
            }
            else if (InvokedVerb == "web")
            {
                StartWebServer = true;
                _logger.DebugFormat("[APP] Reading config file at {0}", Settings.Value.GenerateVerb.ConfigPath);
                Config = File.ReadAllText(Settings.Value.WebVerb.ConfigPath).ToObject<Config>();
                Guard.IsValidConfig(() => Config);
            }
            else
            {
                return false;
            }
            return true;
        }
    }
}