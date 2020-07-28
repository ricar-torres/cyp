using System;
using System.Collections.Generic;

namespace WebApi.Entities
{
    public partial class Products
    {
        public Products()
        {
            ClientProduct = new HashSet<ClientProduct>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual ICollection<ClientProduct> ClientProduct { get; set; }
    }
}
