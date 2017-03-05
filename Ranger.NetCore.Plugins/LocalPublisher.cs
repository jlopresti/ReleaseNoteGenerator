using System.IO;
using log4net;
using Ranger.NetCore.Common;
using Ranger.NetCore.Models;
using Ranger.NetCore.Publisher;

namespace Ranger.NetCore.Plugins
{
    public class LocalPublisher : BasePublisherPlugin<LocalPublishConfig>
    {
        public override string PluginId => "local";

        public LocalPublisher(IReleaseNoteConfiguration configuration)
            : base(configuration)
        {

        }

        public override bool Publish(string releaseNumber, string output)
        {
            File.WriteAllText(Configuration.OutputFile, output);
            return true;
        }
    }
}