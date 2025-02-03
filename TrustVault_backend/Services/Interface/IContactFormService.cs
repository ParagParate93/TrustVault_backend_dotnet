using TrustVault_backend.Entity;

namespace TrustVault_backend.Services.Interface
{
    public interface IContactFormService
    {
        Task SubmitContactFormAsync(ContactForm contactForm);
    }
}
