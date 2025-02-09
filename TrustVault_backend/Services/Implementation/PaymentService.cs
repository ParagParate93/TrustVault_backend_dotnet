using Razorpay.Api;
using TrustVault_backend.Services.Interface;

namespace TrustVault_backend.Services.Implementation
{
    public class PaymentService : IPaymentService
    {
        private readonly string _key;
        private readonly string _secret;

        public PaymentService(IConfiguration configuration)
        {
            _key = configuration["Razorpay:KeyId"];    // Fetching the correct key
            _secret = configuration["Razorpay:KeySecret"]; // Fetching the correct secret
        }

        public string CreateOrder(int amount, string currency = "INR")
        {
            var client = new RazorpayClient(_key, _secret);

            Dictionary<string, object> options = new Dictionary<string, object>
        {
            { "amount", amount }, 
            { "currency", currency },
            { "payment_capture", 1 } 
        };

            Order order = client.Order.Create(options);
            return order["id"].ToString();
        }
    }
}