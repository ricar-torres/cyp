
using System;
using System.Collections.Generic;

namespace server.Dtos
{
  public partial class BonaFidesDto
  {
    public int? Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Siglas { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Benefits { get; set; }
    public string Disclaimer { get; set; }

    public ChapterClientDto Chapter { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
  }
}