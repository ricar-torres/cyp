using System;
using System.Collections.Generic;

namespace WebApi.Entities
{
    public partial class Files
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public string Path { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
