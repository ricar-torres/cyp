using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace WebApi.Entities
{
    public class BaseEntity
    {

        [Column(Order = 1)]
        public int Id { get; set; }

        [Column(Order = 99)]
        public DateTime CreateDt { get; set; }

        [Column(Order = 99)]
        public DateTime UpdDt { get; set; }

        [Column(Order = 99)]
        public int FCreateUserId { get; set; }

        [Column(Order = 99)]
        public int FUpdUserId { get; set; }

        [Column(TypeName = "VARCHAR(100)", Order = 99)]
        public string UpdHostNm { get; set; }

        [JsonIgnore]
        [Column(Order = 99)]
        public DateTime UpdSysHostDt { get; set; }

        [JsonIgnore]
        [Column(TypeName = "VARCHAR(100)", Order = 99)]
        public string UpdSysHostNm { get; set; }

        [JsonIgnore]
        [Column(TypeName = "VARCHAR(100)", Order = 99)]
        public string UpdSysSqlUser { get; set; }

        [JsonIgnore]
        [Column(TypeName = "VARCHAR(15)", Order = 99)]
        public string IPAddress { get; set; }

        [Column(Order = 99)]
        public Boolean DelFlag { get; set; }

    }
}
