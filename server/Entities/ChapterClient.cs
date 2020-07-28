using System;
using System.Collections.Generic;

namespace WebApi.Entities
{
    public partial class ChapterClient
    {
        public int Id { get; set; }
        public int ChapterId { get; set; }
        public int ClientId { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public bool? NewRegistration { get; set; }
        public bool? Primary { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Chapters Chapter { get; set; }
        public virtual Clients Client { get; set; }
    }
}
