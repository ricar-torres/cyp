using System;
using System.Collections.Generic;

namespace WebApi.Entities
{
    public partial class CanceledSubcategories
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CanceledCategoriesId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual CanceledCategories CanceledCategories { get; set; }
    }
}
