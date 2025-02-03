using TrustVault_backend.Entity;
using TrustVault_backend.Repositories.Interface;
using TrustVault_backend.Services.Interface;

namespace TrustVault_backend.Services.Implementation
{
    public class ContactFormService : IContactFormService
    {
        private readonly IContactFormRepository _contactFormRepository;

        public ContactFormService(IContactFormRepository contactFormRepository)
        {
            _contactFormRepository = contactFormRepository;
        }

        public async Task SubmitContactFormAsync(ContactForm contactForm)
        {
            contactForm.SubmittedAt = DateTime.UtcNow;
            await _contactFormRepository.AddContactFormAsync(contactForm);
        }
    }
}


