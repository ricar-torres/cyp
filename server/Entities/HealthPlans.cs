using System;
using System.Collections.Generic;

namespace WebApi.Entities
{
    public partial class HealthPlans
    {
        public HealthPlans()
        {
            Covers = new HashSet<Covers>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual ICollection<Covers> Covers { get; set; }
    }
}
