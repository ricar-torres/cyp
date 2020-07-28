using System;
using System.Collections.Generic;

namespace WebApi.Entities
{
    public partial class Cities
    {
        public Cities()
        {
            Dependents = new HashSet<Dependents>();
            Prospects = new HashSet<Prospects>();
            Zipcodes = new HashSet<Zipcodes>();
        }

        public int Id { get; set; }
        public int CountryId { get; set; }
        public int RegionId { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Countries Country { get; set; }
        public virtual Regions Region { get; set; }
        public virtual ICollection<Dependents> Dependents { get; set; }
        public virtual ICollection<Prospects> Prospects { get; set; }
        public virtual ICollection<Zipcodes> Zipcodes { get; set; }
    }
}
