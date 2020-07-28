using System;
using System.Collections.Generic;

namespace WebApi.Entities
{
    public partial class QualifyingEvents
    {
        public QualifyingEvents()
        {
            Alianzas = new HashSet<Alianzas>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Requirements { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual ICollection<Alianzas> Alianzas { get; set; }
    }
}
