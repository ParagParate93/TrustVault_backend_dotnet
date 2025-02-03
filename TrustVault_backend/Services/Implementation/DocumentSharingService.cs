using TrustVault_backend.Entity;
using TrustVault_backend.Repositories.Interface;
using TrustVault_backend.Services.Interface;
using System;
using System.Threading.Tasks;

namespace TrustVault_backend.Services.Implementation
{
    public class DocumentSharingService : IDocumentSharingService
    {
        private readonly IDocumentSharingRepository _documentSharingRepository;
        private readonly IEmailService _emailService;

        public DocumentSharingService(IDocumentSharingRepository documentSharingRepository, IEmailService emailService)
        {
            _documentSharingRepository = documentSharingRepository;
            _emailService = emailService;
        }

        public async Task<bool> ShareDocumentAsync(DocumentSharingRequest request, DateTime sharedAt)
        {
            string loginLink = "http://localhost:5173/login";
            string subject = $"Document Shared with You: {request.DocumentName} by {request.SharedBy}";
            string body = $"Hello,<br><br><span style='margin-left:40px;'>{request.SharedBy} has shared the document \"{request.DocumentName}\" with you on TrustVault.</span> Please register or log in to your TrustVault account to view the document.<br><span style='margin-left:40px;'>Login here: <a href='{loginLink}'>{loginLink}</a></span>";

             

            var emailSent = await _emailService.SendEmailAsync(request.SharedWith, subject, body);

            return emailSent;
        }

        public async Task SaveDocumentSharingAsync(DocumentSharing documentSharing)
        {
            await _documentSharingRepository.SaveAsync(documentSharing);
        }
    }
}
