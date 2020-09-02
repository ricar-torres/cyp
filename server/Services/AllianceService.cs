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
    Alianzas Update(Alianzas payload);
    void Delete(int id);
    Task<List<HealthPlans>> AvailableHealthPlansForClient(AlianceRequestDto payload);
    Task<List<string>> IsElegible(int clientid);
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

    public async Task<List<HealthPlans>> AvailableHealthPlansForClient(AlianceRequestDto payload)
    {
      var healthPlanList = new List<HealthPlans>();
      Func<string, Task<List<HealthPlans>>> getHP = async type =>
        await (from hp in _context.HealthPlans
               join cv in _context.Covers on hp.Id equals cv.HealthPlanId
               where (type.Contains(cv.Type) || cv.Type == null) && cv.Alianza == true
               select hp).Distinct().ToListAsync();
      var client = await _context.Clients.FirstOrDefaultAsync(x => x.Id == payload.ClientId);
      //calculate client age
      DateTime dob = client.BirthDate.GetValueOrDefault();
      DateTime PresentYear = DateTime.Now;
      TimeSpan ts = PresentYear - dob;
      int years = (new DateTime() + ts).Year - 1;
      //getting informationbased on client age
      if (payload.QualifyingEvetId != 0 || years > 65)
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
        CoverId = payload.CoverId.GetValueOrDefault(),
        CreatedAt = DateTime.Now,
        UpdatedAt = DateTime.Now,
        EndDate = DateTime.Now.AddYears(1)
      };

      await _context.Alianzas.AddRangeAsync(alianza);
      await _context.SaveChangesAsync();

      return alianza;
    }

    public void Delete(int id)
    {
      throw new NotImplementedException();
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
             where pr.ClientId == clientId.GetValueOrDefault()
             select new { alliance = al, AffType = aff, quallifyingEvent = qu, cover = cov })
             .Select(x => new AllianceDto(x.alliance) { Cover = x.cover, QualifyingEvent = x.quallifyingEvent, AffTypeDescription = x.AffType }).ToListAsync();

      clientAlliances.ForEach(x =>
      {
        x.Cover.Alianza = null;
        x.Cover.Alianzas = null;
        x.QualifyingEvent.Alianzas = null;
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

    public Alianzas Update(Alianzas payload)
    {
      throw new NotImplementedException();
    }
  }
}