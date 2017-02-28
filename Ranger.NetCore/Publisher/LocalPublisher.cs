using System.IO;
using log4net;
using Ranger.NetCore.Common;
using Ranger.NetCore.Helpers;
using Ranger.NetCore.Models;
using Ranger.NetCore.Models.Publisher;

namespace Ranger.NetCore.Publisher
{
    [Provider("local", ConfigurationType = typeof(LocalPublishConfig))]
    [ConfigurationParameterValidation("outputfile")]
    internal class LocalPublisher : BasePublisherPlugin<LocalPublishConfig>
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(LocalPublisher));

        public LocalPublisher(IReleaseNoteConfiguration configuration)
            : base(configuration)
        {

        }

        public override bool Publish(string releaseNumber, string output)
        {
            Guard.IsNotNull(() => Configuration);

            File.WriteAllText(Configuration.OutputFile, output);
            return true;
        }
    }
}