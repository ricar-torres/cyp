using System;
using System.Collections.Generic;

namespace WebApi.Entities
{
  public partial class Beneficiaries
  {
    public int Id { get; set; }
    public int? AlianzaId { get; set; }
    public string Name { get; set; }
    public string Gender { get; set; }
    public DateTime BirthDate { get; set; }
    public string Relationship { get; set; }
    public string Percent { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string Ssn { get; set; }
    public int? MultiAssistId { get; set; }
    public virtual Alianzas Alianza { get; set; }
    public virtual MultiAssists MultiAssists { get; set; }


  }
}
