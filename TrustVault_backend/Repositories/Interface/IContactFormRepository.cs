using TrustVault_backend.Entity;

namespace TrustVault_backend.Repositories.Interface
{
    public interface IContactFormRepository
    {
        Task AddContactFormAsync(ContactForm contactForm);
    }
}

