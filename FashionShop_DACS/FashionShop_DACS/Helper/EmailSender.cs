using FashionShop_DACS.Helper.Abstract;
using System.Net.Mail;
using System.Net;

namespace FashionShop_DACS.Helper;

public class EmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;
    public EmailSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task SendEmailAsync(string email, string subject, string body)
    {
        var userName = _configuration["Smtp:Username"];
        var host = _configuration["Smtp:Server"];
        var password = _configuration["Smtp:Password"];

        var smtpClient = new System.Net.Mail.SmtpClient
        {
            Host = host, // set your SMTP server name here
            Port = 587, // Port 
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential()
            {
                UserName = userName,
                Password = password
            }
        };

        using var message = new MailMessage(userName, email)
        {
            Subject = subject,
            Body = body,
        };
        message.From = new MailAddress(userName, "Fashion Shop");
        await smtpClient.SendMailAsync(message);
    }
}
