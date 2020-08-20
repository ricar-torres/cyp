using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace WebApi.Entities
{
  public partial class BonaFides
  {
    public BonaFides()
    {
      Chapters = new HashSet<Chapters>();
    }

    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Siglas { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Benefits { get; set; }
    public string Disclaimer { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public virtual ICollection<Chapters> Chapters { get; set; }
  }
}
