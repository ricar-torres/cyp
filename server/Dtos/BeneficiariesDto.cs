
using System;
using System.Collections.Generic;
using WebApi.Entities;

namespace server.Dtos
{
  public partial class BeneficiariesDto : Beneficiaries
  {
    public BeneficiariesDto() { }
    public BeneficiariesDto(Beneficiaries Beneficiary)
    {
      this.Id = Beneficiary.Id;
      this.AlianzaId = Beneficiary.AlianzaId;
      this.Name = Beneficiary.Name;
      this.Gender = Beneficiary.Gender;
      this.BirthDate = Beneficiary.BirthDate;
      this.Relationship = Beneficiary.Relationship;
      this.Percent = Beneficiary.Percent;
      this.CreatedAt = Beneficiary.CreatedAt;
      this.UpdatedAt = Beneficiary.UpdatedAt;
      this.DeletedAt = Beneficiary.DeletedAt;
      this.Ssn = Beneficiary.Ssn;
      this.MultiAssistId = Beneficiary.MultiAssistId;
      this.Alianza = Beneficiary.Alianza;
      this.MultiAssists = Beneficiary.MultiAssists;
    }
    new public int? Id { get; set; }
  }
}