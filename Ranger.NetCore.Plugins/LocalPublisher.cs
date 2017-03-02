using System.IO;
using log4net;
using Ranger.NetCore.Common;
using Ranger.NetCore.Models;
using Ranger.NetCore.Publisher;

namespace Ranger.NetCore.Plugins
{
    [Provider("local")]
    internal class LocalPublisher : BasePublisherPlugin<LocalPublishConfig>
    {
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