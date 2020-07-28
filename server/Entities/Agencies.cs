using System;
using System.Collections.Generic;

namespace WebApi.Entities
{
    public partial class Agencies
    {
        public Agencies()
        {
            Clients = new HashSet<Clients>();
            Dependents = new HashSet<Dependents>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual ICollection<Clients> Clients { get; set; }
        public virtual ICollection<Dependents> Dependents { get; set; }
    }
}
