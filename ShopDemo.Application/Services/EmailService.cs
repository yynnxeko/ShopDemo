using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using ShopDemo.Application.Interfaces.IServices;
using ShopDemo.Core.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopDemo.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendOtpEmailAsync(string toEmail, string otp)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_settings.FromName, _settings.FromEmail));
            email.To.Add(new MailboxAddress("", toEmail));
            email.Subject = "ShopDemo - Mã OTP của bạn";

            var body = new TextPart("plain")
            {
                Text = $"Mã OTP của bạn là: {otp}\n" +
                       "Mã này có hiệu lực trong 5 phút.\n" +
                       "Nếu bạn không yêu cầu, vui lòng bỏ qua email này."
            };

            email.Body = body;

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_settings.SmtpServer, _settings.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_settings.Username, _settings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
