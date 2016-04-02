using System.IO;
using Ranger.Core.Helpers;
using Ranger.Core.Models;

namespace Ranger.Web.Models
{
    public class ReleaseNoteConfiguration : IReleaseNoteConfiguration
    {
        public ReleaseNoteConfiguration(string configPath)
        {
            Config = File.ReadAllText(configPath).ToObject<Config>();
            Guard.IsValidConfig(() => Config);
        }
        public Config Config { get; private set; }       
    }
}