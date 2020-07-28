using System;
using System.Collections.Generic;

namespace WebApi.Entities
{
    public partial class Chapters
    {
        public Chapters()
        {
            ChapterClient = new HashSet<ChapterClient>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int BonaFideId { get; set; }
        public double Quota { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual BonaFides BonaFide { get; set; }
        public virtual ICollection<ChapterClient> ChapterClient { get; set; }
    }
}
