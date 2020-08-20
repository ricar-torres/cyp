using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace WebApi.Entities
{
  public partial class ClientUser
  {
    public int Id { get; set; }
    public int ClientId { get; set; }
    public int UserId { get; set; }
    public string ConfirmationNumber { get; set; }
    public byte CallType { get; set; }
    public string Comments { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int? AlianzaId { get; set; }
    public virtual Alianzas Alianza { get; set; }
    public virtual Clients Client { get; set; }
    public virtual Users User { get; set; }
  }
}
