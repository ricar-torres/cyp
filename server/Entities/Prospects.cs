using System;
using System.Collections.Generic;

namespace WebApi.Entities
{
    public partial class Prospects
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName1 { get; set; }
        public string LastName2 { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public int CityId { get; set; }
        public string Country { get; set; }
        public int ZipcodeId { get; set; }
        public string Zip4 { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }
        public string Ssn { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Cities City { get; set; }
        public virtual Zipcodes Zipcode { get; set; }
    }
}
