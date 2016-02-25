using System;
using System.IO;
using System.Net.Mail;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReleaseNoteGenerator.Console.Common;
using ReleaseNoteGenerator.Console.Models.Publisher;
using ReleaseNoteGenerator.Console.SourceControl;
using RestSharp.Contrib;

namespace ReleaseNoteGenerator.Console.Publlsher
{
    [Provider("email")]
    internal class EmailPublisher : IPublisher
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(EmailPublisher));
        private EmailPublishConfig _config;

        public EmailPublisher(JObject configPath)
        {
            _config = configPath.ToObject<EmailPublishConfig>();
            if (_config == null)
            {
                _logger.Error("Invalid jira config", new JsonException("Json is invalid"));
                return;
            }
        }

        public bool Publish(string output)
        {
            var smtp = new SmtpClient();
            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_config.From, "Release Note Generator");
            var to = _config.To.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var mail in to)
            {
                mailMessage.To.Add(mail);
            }
            mailMessage.Body = output;
            mailMessage.IsBodyHtml = true;
            smtp.SendAsync(mailMessage, null);
            return true;
        }
    }
}