using System;
using System.Linq;
using System.Net;
using log4net;
using MailKit.Net.Smtp;
using MimeKit;
using Ranger.NetCore.Common;
using Ranger.NetCore.Helpers;
using Ranger.NetCore.Models.publisher;
using Ranger.NetCore.Publisher;

namespace Ranger.NetCore.Smtp.Publisher
{
    [Provider("email", ConfigurationType = typeof(EmailPublishConfig))]
    [ConfigurationParameterValidation("server", "port", "from", "to")]
    internal class EmailPublisher : IPublisher
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(EmailPublisher));
        private EmailPublishConfig _config;

        public EmailPublisher(EmailPublishConfig config)
        {
            _config = config;
            Guard.IsNotNull(() => _config);
        }

        public bool Publish(string release, string output)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_config.From, "Release Note Generator"));
            message.To.AddRange(
                _config.To.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => new MailboxAddress(x))
                        .ToList()
            );
            message.Subject = $"Release note for {release}";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = output;
            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.Connect(_config.Server, _config.Port, _config.Ssl);
                client.AuthenticationMechanisms.Remove("XOAUTH2");    
                client.Authenticate(_config.Username, _config.Password);
                client.Send(message);
                client.Disconnect(true);
            }
            return true;
        }
    }
}