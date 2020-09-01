
using System;
using System.Collections.Generic;
using WebApi.Entities;

namespace server.Dtos
{
  public partial class BeneficiariesDto : Beneficiaries
  {
    new public int? Id { get; set; }
  }
}