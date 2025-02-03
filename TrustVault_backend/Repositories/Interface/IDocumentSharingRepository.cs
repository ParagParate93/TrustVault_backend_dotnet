using TrustVault_backend.Entity;
using System.Threading.Tasks;

namespace TrustVault_backend.Repositories.Interface
{
    public interface IDocumentSharingRepository
    {
        Task SaveAsync(DocumentSharing documentSharing);
    }
}
