using System;
using System.Collections.Generic;

namespace WebApi.Entities
{
    public partial class States
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Countries Country { get; set; }
    }
}
