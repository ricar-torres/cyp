using System;
using WebApi.Entities;

namespace server.Dtos
{
  public class AlianceRequestDto
  {
    public AlianceRequestDto()
    {

    }

    public int ClientId { get; set; }

    public int? QualifyingEvetId { get; set; }

  }
}