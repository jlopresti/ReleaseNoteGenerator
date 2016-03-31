using System;
using System.Net;
using System.Net.Mail;
using log4net;
using Newtonsoft.Json.Linq;
using Ranger.Core.Common;
using Ranger.Core.Helpers;
using Ranger.Core.Models.Publisher;

namespace Ranger.Core.Publisher
{
    [Provider("email")]
    [ConfigurationParameterValidation("server", "port", "from", "to")]
    internal class EmailPublisher : IPublisher
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(EmailPublisher));
        private EmailPublishConfig _config;

        public EmailPublisher(JObject configPath)
        {
            _config = configPath.ToObject<EmailPublishConfig>();
            Guard.IsNotNull(() => _config);
        }

        public bool Publish(string release, string output)
        {
            var smtp = new SmtpClient(_config.Server, _config.Port);
            if (!string.IsNullOrEmpty(_config.Username) && !string.IsNullOrEmpty(_config.Password))
            {
                smtp.Credentials = new NetworkCredential(_config.Username, _config.Password);
            }
            smtp.EnableSsl = _config.Ssl;
            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_config.From, "Release Note Generator");
            var to = _config.To.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var mail in to)
            {
                mailMessage.To.Add(mail);
            }
            mailMessage.Subject = $"Release note for {release}";
            mailMessage.Body = output;
            mailMessage.IsBodyHtml = true;
            smtp.Send(mailMessage);
            return true;
        }
    }
}