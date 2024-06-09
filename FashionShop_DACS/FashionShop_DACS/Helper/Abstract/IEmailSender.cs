namespace FashionShop_DACS.Helper.Abstract;

public interface IEmailSender
{
    Task SendEmailAsync(string email, string subject, string htmlMessage);
}
