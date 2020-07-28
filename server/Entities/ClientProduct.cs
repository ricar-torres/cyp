using System;
using System.Collections.Generic;

namespace WebApi.Entities
{
    public partial class ClientProduct
    {
        public ClientProduct()
        {
            Alianzas = new HashSet<Alianzas>();
        }

        public int Id { get; set; }
        public int ClientId { get; set; }
        public int ProductId { get; set; }
        public byte Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Clients Client { get; set; }
        public virtual Products Product { get; set; }
        public virtual ICollection<Alianzas> Alianzas { get; set; }
    }
}
