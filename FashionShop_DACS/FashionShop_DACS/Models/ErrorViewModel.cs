namespace FashionShop_DACS.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }

    public class TokenModel
    {
        public string Token { get; set; }
    }

    public class ForgotPasswordModel
    {
        public string Email { get; set; }
    }

    public class LockAccountModel
    {
        public string Email { get; set; }
    }

    public class OrderModel
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalPrice { get; set; }

        public string ShippingAddress { get; set; }

        public string Note { get; set; }
    }
}