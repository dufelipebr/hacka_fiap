using MimeKit;
using MailKit.Net.Smtp;
using fiap_hacka.Interfaces;
using fiap_hacka.Entity;
using Microsoft.Extensions.Options;

namespace fiap_hacka.Services
{
    public class EmailService : ISendEmail
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly IWebHostEnvironment _env;
        public EmailService(IOptions<SmtpSettings> smtpSettings, IWebHostEnvironment env)
        {
            _smtpSettings = smtpSettings.Value;
            _env = env;

        }
        public async Task SendEmailAsync(string email, string subject, string body)
        {
            try
            {
                var message = new MimeMessage();

                message.From.Add(new MailboxAddress(_smtpSettings.SenderName,_smtpSettings.SenderEmail));
                message.To.Add(new MailboxAddress("destino", email));
                message.Subject = subject;
                message.Body = new TextPart("html")
                {
                    Text = body
                };

                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    if (_env.IsDevelopment())
                        await client.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port, true);
                    else
                        await client.ConnectAsync(_smtpSettings.Server);

                    await client.AuthenticateAsync(_smtpSettings.Username,
                                                   _smtpSettings.Password);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception e)
            {

                throw new InvalidOperationException(e.Message);
            }
        }
    }
}
