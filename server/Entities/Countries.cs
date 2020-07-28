using System;
using System.Collections.Generic;

namespace WebApi.Entities
{
    public partial class Countries
    {
        public Countries()
        {
            Cities = new HashSet<Cities>();
            States = new HashSet<States>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual ICollection<Cities> Cities { get; set; }
        public virtual ICollection<States> States { get; set; }
    }
}
