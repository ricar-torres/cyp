using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApi.Entities;
using WebApi.Helpers;

namespace WebApi.Services
{
  public interface IAllianceService
  {
    Task<List<Alianzas>> GetAll(int? clientId);
    Alianzas GetById(int id);
    Alianzas Create(Alianzas payload);
    Alianzas Update(Alianzas payload);
    void Delete(int id);
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

    public Alianzas Create(Alianzas payload)
    {
      throw new NotImplementedException();
    }

    public void Delete(int id)
    {
      throw new NotImplementedException();
    }

    public async Task<List<Alianzas>> GetAll(int? clientId)
    {
      if (clientId == null)
      {
        var allAlliances = await _context.Alianzas.ToListAsync();
        return allAlliances;
      }
      var clientAlliances =
      await (from pr in _context.ClientProduct
             join al in _context.Alianzas on pr.Id equals al.ClientProductId
             where pr.ClientId == clientId.GetValueOrDefault()
             select al).ToListAsync();
      return clientAlliances;
    }

    public Alianzas GetById(int id)
    {
      throw new NotImplementedException();
    }

    public Alianzas Update(Alianzas payload)
    {
      throw new NotImplementedException();
    }
  }
}