using System;
using System.Collections.Generic;

namespace WebApi.Entities
{
  public partial class Addresses
  {
    public int? Id { get; set; }
    public int? ClientId { get; set; }
    public byte? Type { get; set; }
    public string Line1 { get; set; }
    public string Line2 { get; set; }
    public string Country { get; set; }
    public string State { get; set; }
    public string City { get; set; }
    public string Zipcode { get; set; }
    public string Zip4 { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
  }
}
