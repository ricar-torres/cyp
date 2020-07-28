using System;
using System.Collections.Generic;

namespace WebApi.Entities
{
    public partial class DocumentCategories
    {
        public DocumentCategories()
        {
            DocumentTypes = new HashSet<DocumentTypes>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual ICollection<DocumentTypes> DocumentTypes { get; set; }
    }
}
