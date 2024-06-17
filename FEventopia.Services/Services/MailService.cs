using AutoMapper.Internal;
using FEventopia.Services.Services.Interfaces;
using FEventopia.Services.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace FEventopia.Services.Services
{
    public class MailService : IMailService
    {
        private readonly MailSetting _mailSettings;

        public MailService(IOptions<MailSetting> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(MailRequestSetting request)
        {
            //Setup email
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(request.ToEmail));
            email.Subject = request.Subject;
            var builder = new BodyBuilder();

            //Body contain the confirm link
            builder.HtmlBody = request.Body; //Using Html file edited instead of request.body
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
