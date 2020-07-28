using System;
using System.Collections.Generic;

namespace WebApi.Entities
{
    public partial class Zipcodes
    {
        public Zipcodes()
        {
            Prospects = new HashSet<Prospects>();
        }

        public int Id { get; set; }
        public int CityId { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Cities City { get; set; }
        public virtual ICollection<Prospects> Prospects { get; set; }
    }
}
