using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrustVault_backend.Entity
{
    public class Document
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public long Size { get; set; }
        public DateTime UploadedAt { get; set; }
        public byte[] EncryptedContent { get; set; }
        public string EncryptionKey { get; set; }
        public string UploadedBy { get; set; }
        public string UploaderEmail { get; set; }
        [NotMapped]
        public bool IsShared { get; set; }
    }
}

