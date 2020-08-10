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
  public interface IClientService
  {
    List<Clients> GetAll();
    Clients GetById(int id);
    Task<ClientInformationDto> Create(ClientInformationDto payload);
    Task<ClientInformationDto> Update(ClientInformationDto payload);
    void Delete(int id);
    Task<Boolean> ChekcName(string criteria);
    Task<List<Addresses>> GetClientAddress(int clientId);
  }

  public class ClientService : IClientService
  {
    private readonly DataContext _context;
    private readonly AppSettings _appSettings;

    public ClientService(DataContext context, IOptions<AppSettings> appSettings)
    {
      _context = context;
      _appSettings = appSettings.Value;
    }

    public List<Clients> GetAll()
    {
      List<Clients> payload = null;

      try
      {

        payload = _context.Clients.Where(ag => ag.DeletedAt == null).ToList();
        payload.ForEach(each =>
        {
          if (each.Ssn != null && each.Ssn.Length >= 9)
          {
            var filteredSsn = each.Ssn.Replace("-", "");
            each.Ssn = "XXX-XX-" + filteredSsn.Substring(5, 4);
          };

        });

      }
      catch (Exception ex)
      {
        throw ex;
      }

      return payload;
    }

    public Clients GetById(int id)
    {
      var res = _context.Clients.FirstOrDefault(c => c.Id == id);
      return res;
    }

    public async Task<ClientInformationDto> Create(ClientInformationDto payload)
    {
      try
      {
        await Task.Delay(100);
        return payload;
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public async Task<ClientInformationDto> Update(ClientInformationDto payload)
    {
      try
      {
        if (payload.Demograpic != null)
        {
          var client = await _context.Clients.AsNoTracking().FirstOrDefaultAsync(cl => cl.Id == payload.Demograpic.Id);
          client.Email = payload.Demograpic.Email;
          client.Gender = payload.Demograpic.Gender;
          client.Initial = payload.Demograpic.Initial;
          client.LastName1 = payload.Demograpic.LastName1;
          client.LastName2 = payload.Demograpic.LastName2;
          client.MaritalStatus = payload.Demograpic.MaritalStatus;
          client.Name = payload.Demograpic.Name;
          client.Phone1 = payload.Demograpic.Phone1;
          client.Phone2 = payload.Demograpic.Phone2;
          client.Phone2 = payload.Demograpic.Phone2;
          client.Ssn = payload.Demograpic.Ssn;

          client.UpdatedAt = DateTime.Now;
          _context.Clients.Update(client);
        }

        if (payload.Address.PhysicalAddress != null)
        {
          var physicalAddress = await _context.Addresses.AsNoTracking().FirstOrDefaultAsync(cl => cl.ClientId == payload.Address.PhysicalAddress.ClientId && cl.Type == payload.Address.PhysicalAddress.Type);
          physicalAddress.Line1 = payload.Address.PhysicalAddress.Line1;
          physicalAddress.Line2 = payload.Address.PhysicalAddress.Line2;
          physicalAddress.State = payload.Address.PhysicalAddress.State;
          physicalAddress.City = payload.Address.PhysicalAddress.City;
          physicalAddress.Type = payload.Address.PhysicalAddress.Type;
          physicalAddress.Zip4 = payload.Address.PhysicalAddress.Zip4;
          physicalAddress.Zipcode = payload.Address.PhysicalAddress.Zipcode;

          physicalAddress.UpdatedAt = DateTime.Now;
          _context.Addresses.Update(physicalAddress);
        }

        if (payload.Address.PostalAddress != null)
        {
          var postalAddress = await _context.Addresses.AsNoTracking().FirstOrDefaultAsync(cl => cl.ClientId == payload.Address.PostalAddress.ClientId && cl.Type == payload.Address.PostalAddress.Type);
          postalAddress.Line1 = payload.Address.PostalAddress.Line1;
          postalAddress.Line2 = payload.Address.PostalAddress.Line2;
          postalAddress.State = payload.Address.PostalAddress.State;
          postalAddress.City = payload.Address.PostalAddress.City;
          postalAddress.Type = payload.Address.PostalAddress.Type;
          postalAddress.Zip4 = payload.Address.PostalAddress.Zip4;
          postalAddress.Zipcode = payload.Address.PostalAddress.Zipcode;

          postalAddress.UpdatedAt = DateTime.Now;
          _context.Addresses.Update(postalAddress);
        }
        await _context.SaveChangesAsync();
        return payload;
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public void Delete(int id)
    {
      var item = _context.Clients.Find(id);

      if (item != null)
      {

        item.DeletedAt = DateTime.Now;

        _context.Clients.Update(item);
        _context.SaveChanges();
      }
      else
        throw new AppException("Agency not found");

    }


    public async Task<Boolean> ChekcName(string criteria)
    {
      if (!String.IsNullOrEmpty(criteria))
      {
        criteria = criteria.ToLower().Trim();
        var payload = await _context.Clients.FirstOrDefaultAsync(ag => ag.Name.ToLower().Trim() == criteria);
        if (payload != null)
        {
          return true;
        }
      }
      return false;
    }

    public async Task<List<Addresses>> GetClientAddress(int clientId)
    {
      try
      {
        var address = await _context.Addresses.Where(addr => addr.ClientId == clientId && addr.DeletedAt == null).ToListAsync();
        return address;
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
  }
}
