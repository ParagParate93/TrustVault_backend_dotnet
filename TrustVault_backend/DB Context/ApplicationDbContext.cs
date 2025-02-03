
using TrustVault_backend.Entity;
using Microsoft.EntityFrameworkCore;

namespace TrustVault_backend.DB_Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ContactForm> ContactForms { get; set; }

        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentSharing> DocumentSharing { get; set; }
        public DbSet<Otp>Otps { get; set; }

    }
}

