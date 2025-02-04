using Microsoft.EntityFrameworkCore;
using TrustVault_backend.DB_Context;
using TrustVault_backend.Entity;
using TrustVault_backend.Services.Interface;
using TrustVault_backend.Util;
using IDocumentRepository = TrustVault_backend.Repositories.Interface.IDocumentRepository;

namespace TrustVault_backend.Services.Implementation
{
    public class DocumentService : IDocumentService
    {
            private readonly IDocumentRepository _documentRepository;
            private readonly ApplicationDbContext _context;
        public DocumentService(IDocumentRepository documentRepository, ApplicationDbContext context)
            {
                _documentRepository = documentRepository;
                _context = context;
        } 

        public async Task<IEnumerable<Document>> GetDocumentsAsync()
        {
            return await _documentRepository.GetAllDocumentsAsync();
        }

        public async Task<Document> GetDocumentByIdAsync(long id)
        {
            return await _documentRepository.GetDocumentByIdAsync(id);
        }

        public async Task CreateDocumentAsync(Document document)
        {
            await _documentRepository.AddDocumentAsync(document);
        }

        public async Task UpdateDocumentAsync(Document document)
        {
            await _documentRepository.UpdateDocumentAsync(document);
        }

        public async Task DeleteDocumentAsync(long id)
        {
            await _documentRepository.DeleteDocumentAsync(id);

        }

        public async Task UploadDocumentAsync(string name, string type, long size, byte[] content, string uploadedBy, string uploaderEmail)
        {
            var (encryptedContent, encryptionKey) = EncryptionHelper.Encrypt(content);

            var document = new Document
            {
                Name = name,
                Type = type,
                Size = size,
                UploadedAt = DateTime.Now,
                EncryptedContent = encryptedContent,
                EncryptionKey = encryptionKey,
                UploadedBy = uploadedBy,
                UploaderEmail = uploaderEmail
            };

            _context.Documents.Add(document);
            await _context.SaveChangesAsync();
        }


        public async Task<List<Document>> GetDocumentsByUploaderAsync(string uploadedBy, string uploaderEmail)
        {
            var uploadedDocuments = await _context.Documents
                                         .Where(d => d.UploadedBy == uploadedBy && d.UploaderEmail == uploaderEmail)
                                         .ToListAsync();

           
            var sharedDocumentsInfo = await _context.DocumentSharing
                                                     .Where(ds => ds.SharedWith == uploaderEmail)
                                                     .ToListAsync();

           
            var sharedDocumentIds = sharedDocumentsInfo.Select(ds => ds.DocumentId).ToList();

            
            var sharedDocuments = await _context.Documents
                                                .Where(d => sharedDocumentIds.Contains(d.Id))
                                                .ToListAsync();

            
            foreach (var document in sharedDocuments)
            {
                document.IsShared = true; 
            }

            
            uploadedDocuments.AddRange(sharedDocuments);

            return uploadedDocuments;
        }
    }

}
