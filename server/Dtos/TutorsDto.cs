

using System;

namespace server.Dtos
{
  public partial class TutorsDto
  {
    public int? Id { get; set; }
    public int? ClientId { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public string PhiFileUrl { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
  }
}