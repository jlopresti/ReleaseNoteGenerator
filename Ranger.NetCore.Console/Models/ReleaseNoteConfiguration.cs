using System.IO;
using log4net;
using Ranger.NetCore.Console.Common;
using Ranger.NetCore.Helpers;
using Ranger.NetCore.Models;

namespace Ranger.NetCore.Console.Models
{
    public class ReleaseNoteConfiguration : BaseConfiguration<ReleaseNoteSettings>
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(ReleaseNoteConfiguration));

        public Config Config { get; private set; }       
        public string ReleaseNumber { get; private set; }
    }
}