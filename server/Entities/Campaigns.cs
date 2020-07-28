using System;
using System.Collections.Generic;

namespace WebApi.Entities
{
    public partial class Campaigns
    {
        public Campaigns()
        {
            Clients = new HashSet<Clients>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Origin { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual ICollection<Clients> Clients { get; set; }
    }
}
