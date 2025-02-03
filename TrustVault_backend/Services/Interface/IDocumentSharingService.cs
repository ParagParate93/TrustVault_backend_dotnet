using TrustVault_backend.Entity;

namespace TrustVault_backend.Services.Interface
{
    public interface IDocumentSharingService
    {
        Task<bool> ShareDocumentAsync(DocumentSharingRequest request, DateTime sharedAt);
        Task SaveDocumentSharingAsync(DocumentSharing documentSharing);
    }
}
