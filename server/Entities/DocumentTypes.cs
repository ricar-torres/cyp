using System;
using System.Collections.Generic;

namespace WebApi.Entities
{
    public partial class DocumentTypes
    {
        public DocumentTypes()
        {
            ClientDocumentType = new HashSet<ClientDocumentType>();
        }

        public int Id { get; set; }
        public int DocumentCategoryId { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual DocumentCategories DocumentCategory { get; set; }
        public virtual ICollection<ClientDocumentType> ClientDocumentType { get; set; }
    }
}
