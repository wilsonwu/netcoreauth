using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace netcoreauth.model
{
    public class Mail
    {
        public string SendEmail(string to, string from, string subject, string content)
        {
            try
            {
                MimeMessage message = new MimeMessage()
                {
                    Subject = subject,
                    Body = new TextPart("Plain") { Text = content }
                };
                message.From.Add(new MailboxAddress(from));
                message.To.Add(new MailboxAddress(to));

                using (var smtp = new SmtpClient())
                {
                    smtp.Connect(
                        "smtp.gmail.com",
                        587,
                        SecureSocketOptions.StartTls
                    );
                    smtp.Authenticate("xxxxxx@imoogoo.com", "xxxxxx");
                    smtp.Send(message);
                    smtp.Disconnect(true);
                }
                return "Success";
            }
            catch (Exception ex)
            {
                return $"Failed. Error: {ex.Message}";
            }
        }

        public async Task<string> SendEmailAsync(string to, string from, string subject, string content)
        {
            try
            {
                MimeMessage message = new MimeMessage()
                {
                    Subject = subject,
                    Body = new TextPart("Plain") { Text = content }
                };
                message.From.Add(new MailboxAddress(from));
                message.To.Add(new MailboxAddress(to));

                using (var smtp = new SmtpClient())
                {
                    await smtp.ConnectAsync(
                        "smtp.gmail.com",
                        587,
                        SecureSocketOptions.StartTls
                    );
                    await smtp.AuthenticateAsync("admin@imoogoo.com", "aaaaaa");
                    await smtp.SendAsync(message);
                    await smtp.DisconnectAsync(true);
                }
                return "Success";
            }
            catch (Exception ex)
            {
                return $"Failed. Error: {ex.Message}";
            }
        }
    }
}
