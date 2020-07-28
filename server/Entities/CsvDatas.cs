using System;
using System.Collections.Generic;

namespace WebApi.Entities
{
    public partial class CsvDatas
    {
        public int Id { get; set; }
        public string CsvFilename { get; set; }
        public bool? CsvHeader { get; set; }
        public string CsvData { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
