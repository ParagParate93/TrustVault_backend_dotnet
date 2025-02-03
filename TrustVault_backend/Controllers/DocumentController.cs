using System.Reflection.Metadata;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TrustVault_backend.Entity;
using TrustVault_backend.Repositories.Implementation.TrustVault.Repositories;
using TrustVault_backend.Repositories.Interface;
using TrustVault_backend.Services.Interface;
using TrustVault_backend.Util;


namespace TrustVault.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[EnableCors("AllowSpecificOrigin")]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly IDocumentSharingService _documentSharingService;
        private readonly IDocumentRepository _documentRepository;
        public DocumentController(IDocumentService documentService, IDocumentSharingService documentSharingService, IDocumentRepository documentRepository)
        {
            _documentService = documentService;
            _documentSharingService = documentSharingService;
            _documentRepository = documentRepository ?? throw new ArgumentNullException(nameof(documentRepository));
        }

        [EnableCors("AllowFrontend")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrustVault_backend.Entity.Document>>> GetDocuments()
        {
            var documents = await _documentService.GetDocumentsAsync();
            return Ok(documents);
        }

        [EnableCors("AllowFrontend")]
        [HttpGet("download/{id}")]
        public async Task<IActionResult> GetDocument(long id)
        {
            try
            {
                var document = await _documentRepository.GetDocumentByIdAsync(id);
                if (document == null)
                {
                    return NotFound("Document not found");
                }

                // Decrypt the document content
                var decryptedContent = EncryptionHelper.Decrypt(document.EncryptedContent, document.EncryptionKey);

                // Set the correct content type and content disposition
                var contentType = document.Type;
                var fileName = document.Name;

                // Check for specific types like text or PDF for inline display
                if (contentType == "text/plain" || contentType == "application/pdf")
                {
                    // Content-Disposition: inline for previewing
                    Response.Headers.Add("Content-Disposition", $"inline; filename={fileName}");
                }
                else
                {
                    // For other file types, set it as an attachment for downloading
                    Response.Headers.Add("Content-Disposition", $"attachment; filename={fileName}");
                }

                // Return the file with the appropriate headers and content type
                return File(decryptedContent, contentType, fileName, enableRangeProcessing: true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error downloading document: {ex.Message}");
                return StatusCode(500, "Failed to download the document");
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateDocument([FromBody] TrustVault_backend.Entity.Document document)
        {
            await _documentService.CreateDocumentAsync(document);
            return CreatedAtAction(nameof(GetDocument), new { id = document.Id }, document);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateDocument(int id, [FromBody] TrustVault_backend.Entity.Document document)
        {
            if (id != document.Id)
            {
                return BadRequest();
            }

            await _documentService.UpdateDocumentAsync(document);
            return Ok(document);
        }
        [EnableCors("AllowFrontend")]
        [HttpDelete("deleteDocument/{id}")]
        public async Task<ActionResult> DeleteDocument(long id)
        {
            await _documentService.DeleteDocumentAsync(id);
            return NoContent();
        }


        [HttpGet("test")]
        public ActionResult TestApi()
        {
            var dummyResponse = new
            {
                Message = "This is a dummy API for testing purposes.",
                Timestamp = DateTime.UtcNow,
                Status = "Success"
            };
            return Ok(dummyResponse);
        }

        [EnableCors("AllowFrontend")]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file, [FromForm] string uploadedBy, [FromForm] string uploaderEmail)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File is required.");
            }

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            var fileContent = memoryStream.ToArray();
            await _documentService.UploadDocumentAsync(file.FileName, file.ContentType, file.Length, fileContent, uploadedBy, uploaderEmail);

            return Ok(new { message = "File uploaded successfully." });
        }
        [EnableCors("AllowFrontend")]
        [HttpGet("getAllDocument")]
        public async Task<IActionResult> GetDocumentsByUploader([FromQuery] string uploadedBy, [FromQuery] string uploaderEmail)
        {
            try
            {
               
                var documents = await _documentService.GetDocumentsByUploaderAsync(uploadedBy, uploaderEmail);

               
                if (documents != null && documents.Any())
                {
                    return Ok(documents);
                }

               
                return StatusCode(500, new { message = "No documents found for the given uploader and email." });
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, new { message = "An error occurred while fetching the documents.", details = ex.Message });
            }
        }
        [EnableCors("AllowFrontend")]
        [HttpPost("share")]
        public async Task<IActionResult> ShareDocument([FromBody] DocumentSharingRequest request)
        {
            try
            {
                // Fetch the document by documentId from the request
                var document = await _documentService.GetDocumentByIdAsync(request.DocumentId);
                if (document == null)
                {
                    return NotFound("Document not found.");
                }

                // Create the sharing record and save it
                var documentSharing = new DocumentSharing
                {
                    DocumentId = request.DocumentId,
                    SharedBy = request.SharedBy,
                    SharedWith = request.SharedWith,
                    SharedAt = DateTime.Now,
                    DocumentName = document.Name
                };

                // Save sharing record in the database
                await _documentSharingService.SaveDocumentSharingAsync(documentSharing);

                // Send the email
                var isEmailSent = await _documentSharingService.ShareDocumentAsync(request, DateTime.Now);
                if (!isEmailSent)
                {
                    return StatusCode(500, "Failed to send email.");
                }

                return Ok("Document shared successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to share document: {ex.Message}");
            }
        }

        
    }
}
