using System;
using WebApi.Entities;

namespace server.Dtos
{
  public class AllianceDto : Alianzas
  {
    public AllianceDto()
    {

    }
    public AllianceDto(Alianzas Alianza)
    {
      this.Id = Alianza.Id;
      this.ClientProductId = Alianza.ClientProductId;
      this.QualifyingEventId = Alianza.QualifyingEventId;
      this.CoverId = Alianza.CoverId;
      this.StartDate = Alianza.StartDate;
      this.ElegibleDate = Alianza.ElegibleDate;
      this.EndDate = Alianza.EndDate;
      this.EndReason = Alianza.EndReason;
      this.AffType = Alianza.AffType;
      this.AffStatus = Alianza.AffStatus;
      this.AffFlag = Alianza.AffFlag;
      this.Coordination = Alianza.Coordination;
      this.LifeInsurance = Alianza.LifeInsurance;
      this.MajorMedical = Alianza.MajorMedical;
      this.Prima = Alianza.Prima;
      this.CreatedAt = Alianza.CreatedAt;
      this.UpdatedAt = Alianza.UpdatedAt;
      this.DeletedAt = Alianza.DeletedAt;
      this.Joint = Alianza.Joint;
      this.CoverAmount = Alianza.CoverAmount;
      this.LifeInsuranceAmount = Alianza.LifeInsuranceAmount;
      this.MajorMedicalAmount = Alianza.MajorMedicalAmount;
      this.SubTotal = Alianza.SubTotal;
      this.ClientProduct = Alianza.ClientProduct;
      this.Cover = Alianza.Cover;
      this.QualifyingEvent = Alianza.QualifyingEvent;
      this.Beneficiaries = Alianza.Beneficiaries;
      this.ClientUser = Alianza.ClientUser;
    }
    public AffType AffTypeDescription { get; set; }
  }
}