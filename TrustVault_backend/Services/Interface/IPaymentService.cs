namespace TrustVault_backend.Services.Interface
{
    public interface IPaymentService
    {
        string CreateOrder(int amount, string currency = "INR");
    }
}
