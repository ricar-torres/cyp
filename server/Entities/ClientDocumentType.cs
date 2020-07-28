using System;
using System.Collections.Generic;

namespace WebApi.Entities
{
    public partial class ClientDocumentType
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int DocumentTypeId { get; set; }
        public string Url { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string SobImgUrl { get; set; }

        public virtual Clients Client { get; set; }
        public virtual DocumentTypes DocumentType { get; set; }
    }
}
