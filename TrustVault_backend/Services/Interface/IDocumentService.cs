using TrustVault_backend.Entity;


namespace TrustVault_backend.Services.Interface
{
    public interface IDocumentService
    {
        Task<IEnumerable<Document>> GetDocumentsAsync();
        Task<Document> GetDocumentByIdAsync(long id);
        Task CreateDocumentAsync(Document document);
        Task UpdateDocumentAsync(Document document);
        Task DeleteDocumentAsync(long id);
        Task UploadDocumentAsync(string name, string type, long size, byte[] content, string uploadedBy, string uploaderEmail);
        Task<List<Document>> GetDocumentsByUploaderAsync(string uploadedBy, string uploaderEmail);

    }
}
