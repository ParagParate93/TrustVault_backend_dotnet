using TrustVault_backend.DB_Context;
using TrustVault_backend.Repositories.Interface;
using TrustVault_backend.Entity;
using Microsoft.EntityFrameworkCore;

namespace TrustVault_backend.Repositories.Implementation
{
    namespace TrustVault.Repositories
    {
        public class DocumentRepository : IDocumentRepository
        {
            private readonly ApplicationDbContext _context;

            public DocumentRepository(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<Document>> GetAllDocumentsAsync()
            {
                return await _context.Documents.ToListAsync();
            }

            public async Task<Document> GetDocumentByIdAsync(long id)
            {
                return await _context.Documents.FindAsync(id);
            }

            public async Task AddDocumentAsync(Document document)
            {
                _context.Documents.Add(document);
                await _context.SaveChangesAsync();
            }

            public async Task UpdateDocumentAsync(Document document)
            {
                _context.Documents.Update(document);
                await _context.SaveChangesAsync();
            }

            public async Task DeleteDocumentAsync(long id)
            {
                var document = await _context.Documents.FindAsync(id);
                if (document != null)
                {
                    _context.Documents.Remove(document);
                    await _context.SaveChangesAsync();

                }
            }
        }
    }

}
