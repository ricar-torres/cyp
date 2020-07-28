using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Entities.Identity
{
    public class Application
    {
        public int Id { get; set; }

        [Column(TypeName = "VARCHAR(20)")]
        public string Name { get; set; }

        [Column(TypeName = "VARCHAR(255)")]
        public string Key { get; set; }

        [Column(TypeName = "VARCHAR(6)")]
        public string PrimaryColor { get; set; }

        public bool DelFlag { get; set; }

        [Column(TypeName = "VARCHAR(255)")]
        public string EmailApiKey { get; set; }

        [Column(TypeName = "VARCHAR(254)")]
        public string SenderEmail { get; set; }

    }
}
