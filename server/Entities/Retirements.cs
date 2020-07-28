using System;
using System.Collections.Generic;

namespace WebApi.Entities
{
    public partial class Retirements
    {
        public Retirements()
        {
            Clients = new HashSet<Clients>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual ICollection<Clients> Clients { get; set; }
    }
}
