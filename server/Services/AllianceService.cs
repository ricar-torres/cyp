using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using server.Dtos;
using WebApi.Entities;
using WebApi.Helpers;


namespace WebApi.Services
{
  public interface IAllianceService
  {
    Task<List<AllianceDto>> GetAll(int? clientId);
    Alianzas GetById(int id);
    Task<Alianzas> Create(AllianceDto payload);
    Task Update(AllianceDto payload);
    Task Delete(int id);
    Task<List<HealthPlans>> AvailableHealthPlansForClient(int clientId);
    Task<List<string>> IsElegible(int clientid);
    Task<List<AffType>> GetAllAffTypes();
  }

  public class AllianceService : IAllianceService
  {

    private readonly DataContext _context;
    private readonly AppSettings _appSettings;

    public AllianceService(DataContext context, IOptions<AppSettings> appSettings)
    {
      _context = context;
      _appSettings = appSettings.Value;
    }

    public async Task<List<HealthPlans>> AvailableHealthPlansForClient(int clientId)
    {
      var healthPlanList = new List<HealthPlans>();
      Func<string, Task<List<HealthPlans>>> getHP = async type =>
        await (from hp in _context.HealthPlans
               join cv in _context.Covers on hp.Id equals cv.HealthPlanId
               where (type.Contains(cv.Type) || cv.Type == null) && cv.Alianza == true
               select hp).Distinct().ToListAsync();
      var client = await _context.Clients.FirstOrDefaultAsync(x => x.Id == clientId);
      //calculate client age
      // DateTime dob = client.BirthDate.GetValueOrDefault();
      // DateTime PresentYear = DateTime.Now;
      // TimeSpan ts = PresentYear - dob;
      // int years = (new DateTime() + ts).Year - 1;
      //getting informationbased on client age
      var medicareA = client.MedicareA == false || client.MedicareA == null ? false : true;
      var medicareB = client.MedicareB == false || client.MedicareB == null ? false : true;
      if (medicareA && medicareB)
      {
        healthPlanList = await getHP("+65,-65");
      }
      else
      {
        healthPlanList = await getHP("-65");
      }

      return healthPlanList;
    }

    public async Task<Alianzas> Create(AllianceDto payload)
    {
      var afftype = 1;
      //setting aff type
      afftype = await defineAfftype(payload, afftype);

      //adding productclient to fill required field in aliance
      var clientProduct = new ClientProduct()
      {
        ClientId = payload.ClientId.GetValueOrDefault(),
        CreatedAt = DateTime.Now,
        UpdatedAt = DateTime.Now,
        ProductId = 1,
        Status = 0
      };
      await _context.ClientProduct.AddAsync(clientProduct);
      await _context.SaveChangesAsync();

      // creating actual aliance
      var alianza = new Alianzas()
      {
        ClientProductId = clientProduct.Id,
        QualifyingEventId = payload.QualifyingEventId == null ? 1 : payload.QualifyingEventId.Value,
        CoverId = payload.CoverId.GetValueOrDefault(),
        AffType = (byte?)afftype,
        AffStatus = 1,
        ElegibleDate = DateTime.Now,
        StartDate = DateTime.Now,
        CreatedAt = DateTime.Now,
        UpdatedAt = DateTime.Now,
        EndDate = DateTime.Now.AddYears(1)
      };

      //TODO: check if the status of the aliance is complete or pending
      //for the moment is complete
      alianza.AffStatus = 1;
      await _context.Alianzas.AddAsync(alianza);
      await _context.SaveChangesAsync();

      foreach (var item in payload.Beneficiaries)
      {
        var beneficiary = new Beneficiaries()
        {
          Name = item.Name,
          BirthDate = item.BirthDate,
          Gender = item.Gender,
          Percent = item.Percent,
          Relationship = item.Relationship,
          CreatedAt = DateTime.Now,
          UpdatedAt = DateTime.Now,
          AlianzaId = alianza.Id,
          Ssn = item.Ssn
        };
        await _context.Beneficiaries.AddAsync(beneficiary);
      }

      foreach (var item in payload.AddonList)
      {
        var addonId = item;
        await _context.AlianzaAddOns.AddAsync(new AlianzaAddOns() { AlianzaId = alianza.Id, InsuranceAddOnId = addonId });
      }

      await _context.SaveChangesAsync();
      return alianza;
    }

    private async Task<int> defineAfftype(AllianceDto payload, int afftype)
    {
      var lastAliance = await _context.ClientProduct
      .Join(_context.Alianzas.Include(x => x.Cover).ThenInclude(s => s.HealthPlan)
      , c => c.Id,
      a => a.ClientProductId,
      (c, a) => a).FirstOrDefaultAsync(s => s.ClientProduct.ClientId == payload.ClientId);
      var newPlan = await _context.Covers.Include(x => x.HealthPlan).FirstOrDefaultAsync(x => x.Id == payload.CoverId);


      if (lastAliance?.Cover?.HealthPlan?.Id == null)
      {
        afftype = 1;
      }
      else if (lastAliance?.Cover?.HealthPlan?.Name == newPlan.Name)
      {
        if (lastAliance?.Cover?.Id == payload.CoverId)
          afftype = 3;
        else
          afftype = 2;
      }
      else if (lastAliance?.Cover?.HealthPlan?.Id != newPlan.Id)
      {
        afftype = 6;
      }

      return afftype;
    }

    public async Task Delete(int id)
    {
      var alliance = await _context.Alianzas.FirstOrDefaultAsync(x => x.Id == id);
      alliance.DeletedAt = DateTime.Today;
      _context.Alianzas.Update(alliance);
      await _context.SaveChangesAsync();
    }

    public async Task<List<AllianceDto>> GetAll(int? clientId)
    {
      if (clientId == null)
      {
        var allAlliances = await _context.Alianzas.Select(x => new AllianceDto(x)).ToListAsync();
        return allAlliances;
      }
      var clientAlliances =
      await (from pr in _context.ClientProduct
             join al in _context.Alianzas on pr.Id equals al.ClientProductId
             join aff in _context.AffType on al.AffType.ToString() equals aff.Id.ToString()
             join cov in _context.Covers on al.CoverId equals cov.Id
             join qu in _context.QualifyingEvents on al.QualifyingEventId equals qu.Id
             where pr.ClientId == clientId.GetValueOrDefault() && al.DeletedAt == null
             select new { alliance = al, AffType = aff, quallifyingEvent = qu, cover = cov })
             .Select(x => new AllianceDto(x.alliance) { Cover = x.cover, QualifyingEvent = x.quallifyingEvent, AffTypeDescription = x.AffType }).ToListAsync();
      //removing loop reference and adding additional info
      clientAlliances.ForEach((x) =>
      {
        x.Cover.Alianza = null;
        x.Cover.Alianzas = null;
        x.QualifyingEvent.Alianzas = null;
        x.AddonList = _context.AlianzaAddOns.Where(s => s.AlianzaId == x.Id).Select(s => s.InsuranceAddOnId).ToList();
        x.Beneficiaries = _context.Beneficiaries.Where(s => s.AlianzaId == x.Id && s.DeletedAt == null).Select(s => new BeneficiariesDto(s)).ToList();
        x.Beneficiaries.ForEach(x =>
        {
          x.Alianza = null;
        });
      });
      return clientAlliances;
    }

    public Alianzas GetById(int id)
    {
      throw new NotImplementedException();
    }

    public async Task<List<string>> IsElegible(int clientid)
    {
      var declineList = new List<string>();
      var client = await _context.Clients.Include(c => c.ChapterClient).FirstOrDefaultAsync(x => x.Id == clientid);
      if (client != null)
      {

        if (client.Ssn == null || (client.Ssn != null && client?.Ssn?.Length == 4))
        {
          declineList.Add("SSNNOTCOMPLETE");
        }

        if (client.BirthDate == null)
        {
          declineList.Add("MISSINGDOB");
        }

        if (client.ChapterClient.FirstOrDefault() == null)
        {
          declineList.Add("MISSINGBONAFIDES");
        }

      }
      else
      {
        throw new Exception("No client with provided Id");
      }

      if (declineList.FirstOrDefault() != null)
        return declineList;
      return null;

    }

    public async Task<List<AffType>> GetAllAffTypes()
    {
      var afftypes = await _context.AffType.ToListAsync();
      return afftypes;
    }

    public async Task Update(AllianceDto payload)
    {
      var aliance = await _context.Alianzas.FirstOrDefaultAsync(a => a.Id == payload.Id);
      aliance.StartDate = payload.StartDate;
      aliance.ElegibleDate = payload.ElegibleDate.GetValueOrDefault();
      aliance.AffType = payload.AffType;
      aliance.AffStatus = payload.AffStatus.GetValueOrDefault();
      aliance.CoverId = payload.CoverId.GetValueOrDefault();
      UpdateBeneficiaries(payload);
      UpdateAddons(payload);
      await _context.SaveChangesAsync();
    }

    private void UpdateAddons(AllianceDto payload)
    {
      var existingAddons = _context.AlianzaAddOns.Where(s => s.AlianzaId == payload.Id).Select(s => s).ToList();
      existingAddons.ForEach(x =>
      {
        var exist = payload.AddonList.Contains(x.InsuranceAddOnId);
        if (exist == false)
        {
          _context.AlianzaAddOns.Remove(x);
        };
      });

      payload.AddonList.ForEach(x =>
      {
        var exist = existingAddons.FirstOrDefault(ad => ad.InsuranceAddOnId == x);
        if (exist == null)
        {
          var newAddon = new AlianzaAddOns() { AlianzaId = payload.Id.GetValueOrDefault(), InsuranceAddOnId = x };
          _context.AlianzaAddOns.Add(newAddon);
        }
      });
    }

    private void UpdateBeneficiaries(AllianceDto payload)
    {
      payload.Beneficiaries.ForEach(x =>
      {
        if (x.Id != null)
        {
          var beneficiary = _context.Beneficiaries.FirstOrDefault(b => b.Id == x.Id);
          beneficiary.Name = x.Name;
          beneficiary.Ssn = x.Ssn;
          beneficiary.BirthDate = x.BirthDate;
          beneficiary.Gender = x.Gender;
          beneficiary.Relationship = x.Relationship;
          beneficiary.Percent = x.Percent;
          beneficiary.UpdatedAt = DateTime.Now;
          _context.Beneficiaries.Update(beneficiary);
        }
        else
        {
          var beneficiary = new Beneficiaries();
          beneficiary.AlianzaId = payload.Id;
          beneficiary.Name = x.Name;
          beneficiary.Ssn = x.Ssn;
          beneficiary.BirthDate = x.BirthDate;
          beneficiary.Gender = x.Gender;
          beneficiary.Relationship = x.Relationship;
          beneficiary.Percent = x.Percent;
          beneficiary.UpdatedAt = DateTime.Now;
          _context.Beneficiaries.Add(beneficiary);
        }
      });
      var existing = _context.Beneficiaries.Where(b => b.AlianzaId == payload.Id).ToList();
      existing.ForEach(ex =>
      {
        var exist = payload.Beneficiaries.FirstOrDefault(b => b.Ssn == ex.Ssn);
        if (exist == null)
        {
          ex.DeletedAt = DateTime.Now;
          _context.Beneficiaries.Update(ex);
        }
      });
    }
  }
}