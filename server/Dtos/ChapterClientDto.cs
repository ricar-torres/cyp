
using System;

namespace server.Dtos
{
  public partial class ChapterClientDto
  {
    public int Id { get; set; }
    public int ChapterId { get; set; }
    public int ClientId { get; set; }
    public DateTime? RegistrationDate { get; set; }
    public bool? NewRegistration { get; set; }
    public bool? Primary { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
  }
}