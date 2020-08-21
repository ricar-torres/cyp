using System;
using server.Entities;
using WebApi.Entities;

namespace server.Dtos
{
  public class DependentDto : Dependents
  {
    public DependentDto()
    {

    }
    public DependentDto(Dependents dependent) : base()
    {
      this.Id = dependent.Id;
      this.Name = dependent.Name;
      this.Initial = dependent.Initial;
      this.LastName1 = dependent.LastName1;
      this.LastName2 = dependent.LastName2;
      this.Phone1 = dependent.Phone1;
      this.Phone2 = dependent.Phone2;
      this.Ssn = dependent.Ssn;
      this.UpdatedAt = dependent.UpdatedAt;
      this.Agency = dependent.Agency;
      this.AgencyId = dependent.AgencyId;
      this.BirthDate = dependent.BirthDate;
      this.City = dependent.City;
      this.CityId = dependent.CityId;
      this.Client = dependent.Client;
      this.ClientId = dependent.ClientId;
      this.ContractNumber = dependent.ContractNumber;
      this.Cover = dependent.Cover;
      this.CoverId = dependent.CoverId;
      this.CreatedAt = dependent.CreatedAt;
      this.DeletedAt = dependent.DeletedAt;
      this.EffectiveDate = dependent.EffectiveDate;
      this.Email = dependent.Email;
      this.Gender = dependent.Gender;
    }
    new public int? Id { get; set; }
    new public int? ClientId { get; set; }
    public new TypeOfRelationship Relationship { get; set; }
  }
}