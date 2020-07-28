using System;
using System.Collections.Generic;

namespace WebApi.Entities
{
    public partial class ActivityLog
    {
        public int Id { get; set; }
        public string LogName { get; set; }
        public string Description { get; set; }
        public int? SubjectId { get; set; }
        public string SubjectType { get; set; }
        public int? CauserId { get; set; }
        public string CauserType { get; set; }
        public string Properties { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
