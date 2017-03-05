using System;
using System.Linq;
using log4net;
using MailKit.Net.Smtp;
using MimeKit;
using Ranger.NetCore.Common;
using Ranger.NetCore.Helpers;
using Ranger.NetCore.Models;
using Ranger.NetCore.Publisher;

namespace Ranger.NetCore.Smtp.Publisher
{
    public class SmtpPublisher : BasePublisherPlugin<SmtpPublishConfig>
    {
        public override string PluginId => "smtp";

        readonly ILog _logger = LogManager.GetLogger(typeof(SmtpPublisher));

        public SmtpPublisher(IReleaseNoteConfiguration configuration)
            : base(configuration)
        {
            
        }

        public override bool Publish(string release, string output)
        {
            if (string.IsNullOrEmpty(release) || string.IsNullOrEmpty(output))
            {
                return false;
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(Configuration.From, "Release Note Generator"));
            message.To.AddRange(
                Configuration.To.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => new MailboxAddress(x))
                        .ToList()
            );
            message.Subject = $"Release note for {release}";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = output;
            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.Connect(Configuration.Server, Configuration.Port, Configuration.Ssl);
                client.AuthenticationMechanisms.Remove("XOAUTH2");    
                client.Authenticate(Configuration.Username, Configuration.Password);
                client.Send(message);
                client.Disconnect(true);
            }
            return true;
        }
    }
}