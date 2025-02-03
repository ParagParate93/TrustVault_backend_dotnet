using System.Threading.Tasks;

namespace TrustVault_backend.Services.Interface
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string to, string subject, string body);
    }
}