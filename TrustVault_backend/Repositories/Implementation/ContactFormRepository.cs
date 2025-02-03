using TrustVault_backend.DB_Context;
using TrustVault_backend.Entity;
using TrustVault_backend.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace TrustVault_backend.Repositories.Implementation
{
    public class ContactFormRepository : IContactFormRepository
    {
        private readonly ApplicationDbContext _context;

        public ContactFormRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddContactFormAsync(ContactForm contactForm)
        {
            await _context.ContactForms.AddAsync(contactForm);
            await _context.SaveChangesAsync();
        }
    }
}


