using TrustVault_backend.Entity;
using TrustVault_backend.Repositories.Interface;
using TrustVault_backend.Services.Interface;

namespace TrustVault_backend.Services.Implementation
{
    public class ContactFormService : IContactFormService
    {
        private readonly IContactFormRepository _contactFormRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService; // Inject Email Service
    

        public ContactFormService(IContactFormRepository contactFormRepository, IUserRepository userRepository, IEmailService emailService)
        {
            _contactFormRepository = contactFormRepository;
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public async Task SubmitContactFormAsync(ContactForm contactForm)
        {
            contactForm.SubmittedAt = DateTime.UtcNow;
            await _contactFormRepository.AddContactFormAsync(contactForm);

            // Fetch all admins
            List<User> admins = await _userRepository.GetAdminsAsync();

            // Email subject & content
            string subject = "New Contact Us Message";
            string emailBody = $"<p><b>Name:</b> {contactForm.Name}</p>" +
                               $"<p><b>Email:</b> {contactForm.Email}</p>" +
                               $"<p><b>Message:</b> {contactForm.Message}</p>" +
                               $"<p><b>Submitted At:</b> {contactForm.SubmittedAt}</p>";

            // Send email to all admins
            foreach (var admin in admins)
            {
                await _emailService.SendEmailAsync(admin.Email, subject, emailBody);
            }

        }
    }
}


