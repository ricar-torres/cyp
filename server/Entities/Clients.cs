using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Entities
{
  public partial class Clients
  {
    public Clients()
    {
      ChapterClient = new HashSet<ChapterClient>();
      ClientCommunicationMethod = new HashSet<ClientCommunicationMethod>();
      ClientDocumentType = new HashSet<ClientDocumentType>();
      ClientProduct = new HashSet<ClientProduct>();
      ClientUser = new HashSet<ClientUser>();
      Dependents = new HashSet<Dependents>();
      Tutors = new HashSet<Tutors>();
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Initial { get; set; }
    public string LastName1 { get; set; }
    public string LastName2 { get; set; }
    public string Ssn { get; set; }
    public byte? Gender { get; set; }
    public DateTime? BirthDate { get; set; }
    public string Email { get; set; }
    public string Phone1 { get; set; }
    public string Phone2 { get; set; }
    public byte? MaritalStatus { get; set; }
    public int AgencyId { get; set; }
    public double? Contribution { get; set; }
    public int RetirementId { get; set; }
    public int CoverId { get; set; }
    public string ContractNumber { get; set; }
    public DateTime? EffectiveDate { get; set; }
    public bool? MedicareA { get; set; }
    public bool? MedicareB { get; set; }
    public int CampaignId { get; set; }
    public bool? Principal { get; set; }
    public byte Status { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool? PreRegister { get; set; }
    public virtual Agencies Agency { get; set; }
    public virtual Campaigns Campaign { get; set; }
    public virtual Covers Cover { get; set; }
    public virtual Retirements Retirement { get; set; }
    public virtual ICollection<ChapterClient> ChapterClient { get; set; }
    public virtual ICollection<ClientCommunicationMethod> ClientCommunicationMethod { get; set; }
    public virtual ICollection<ClientDocumentType> ClientDocumentType { get; set; }
    public virtual ICollection<ClientProduct> ClientProduct { get; set; }
    public virtual ICollection<ClientUser> ClientUser { get; set; }
    public virtual ICollection<Dependents> Dependents { get; set; }
    public virtual ICollection<Tutors> Tutors { get; set; }
  }
}
