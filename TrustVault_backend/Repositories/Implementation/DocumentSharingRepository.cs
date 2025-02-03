using Microsoft.EntityFrameworkCore;
using TrustVault_backend.DB_Context;
using TrustVault_backend.Entity;
using TrustVault_backend.Repositories.Interface;
using System.Threading.Tasks;

namespace TrustVault_backend.Repositories.Implementation
{
    public class DocumentSharingRepository : IDocumentSharingRepository
    {
        private readonly ApplicationDbContext _context;

        public DocumentSharingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SaveAsync(DocumentSharing documentSharing)
        {
            await _context.DocumentSharing.AddAsync(documentSharing);
            await _context.SaveChangesAsync();
        }
    }
}
