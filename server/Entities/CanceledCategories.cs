using System;
using System.Collections.Generic;

namespace WebApi.Entities
{
    public partial class CanceledCategories
    {
        public CanceledCategories()
        {
            CanceledSubcategories = new HashSet<CanceledSubcategories>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int CanceledReasonsId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual CanceledReasons CanceledReasons { get; set; }
        public virtual ICollection<CanceledSubcategories> CanceledSubcategories { get; set; }
    }
}
