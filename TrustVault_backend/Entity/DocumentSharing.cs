using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrustVault_backend.Entity
{
    public class DocumentSharing
    {
        [Key]
        public long Id { get; set; }

        public long DocumentId { get; set; }

        public string SharedBy { get; set; }

        public string SharedWith { get; set; }

        public DateTime SharedAt { get; set; }

        public string DocumentName { get; set; }
    }

    // Request DTO for sharing a document
    public class DocumentSharingRequest
    {
        public long DocumentId { get; set; }
        public string SharedBy { get; set; }
        public string SharedWith { get; set; }
        public string DocumentName { get; set; }
    }
}
