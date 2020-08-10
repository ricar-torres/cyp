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
        var newClient = new Clients();
        if (payload.PreRegister)
        {
          newClient.Email = payload.Demographic?.Email;
          newClient.Gender = payload.Demographic?.Gender;
          newClient.Initial = payload.Demographic?.Initial;
          newClient.LastName1 = payload.Demographic?.LastName1;
          newClient.LastName2 = payload.Demographic?.LastName2;
          newClient.MaritalStatus = payload.Demographic?.MaritalStatus;
          newClient.Name = payload.Demographic?.Name;
          newClient.Phone1 = payload.Demographic?.Phone1;
          newClient.Phone2 = payload.Demographic?.Phone2;
          newClient.Ssn = payload.Demographic?.Ssn;
          newClient.BirthDate = payload.Demographic?.BirthDate;
          newClient.AgencyId = 17;
          newClient.RetirementId = 6;
          newClient.CoverId = 206;
          newClient.CampaignId = 17;
          newClient.PreRegister = true;

          newClient.UpdatedAt = DateTime.Now;
          newClient.CreatedAt = DateTime.Now;
          await _context.Clients.AddAsync(newClient);
        }
        else
        {

        }
        await _context.SaveChangesAsync();
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
        if (payload.Demographic != null)
        {
          var client = await _context.Clients.AsNoTracking().FirstOrDefaultAsync(cl => cl.Id == payload.Demographic.Id);
          client.Email = payload.Demographic?.Email;
          client.Gender = payload.Demographic?.Gender;
          client.Initial = payload.Demographic?.Initial;
          client.LastName1 = payload.Demographic?.LastName1;
          client.LastName2 = payload.Demographic?.LastName2;
          client.MaritalStatus = payload.Demographic?.MaritalStatus;
          client.Name = payload.Demographic?.Name;
          client.Phone1 = payload.Demographic?.Phone1;
          client.Phone2 = payload.Demographic?.Phone2;
          client.Phone2 = payload.Demographic?.Phone2;
          client.Ssn = payload.Demographic?.Ssn;

          client.UpdatedAt = DateTime.Now;
          _context.Clients.Update(client);
        }

        if (payload.Address?.PhysicalAddress != null)
        {
          var physicalAddress = await _context.Addresses.AsNoTracking().FirstOrDefaultAsync(cl => cl.ClientId == payload.Demographic.Id && cl.Type == 1);

          if (physicalAddress != null)
          {
            physicalAddress.Line1 = payload.Address?.PhysicalAddress.Line1;
            physicalAddress.Line2 = payload.Address?.PhysicalAddress.Line2;
            physicalAddress.State = payload.Address?.PhysicalAddress.State;
            physicalAddress.City = payload.Address?.PhysicalAddress.City;
            physicalAddress.Type = payload.Address?.PhysicalAddress.Type;
            physicalAddress.Zip4 = payload.Address?.PhysicalAddress.Zip4;
            physicalAddress.Zipcode = payload.Address?.PhysicalAddress.Zipcode;
            physicalAddress.ClientId = payload.Demographic.Id;
            _context.Addresses.Update(physicalAddress);
          }
          else
          {
            physicalAddress = new Addresses();
            physicalAddress.Line1 = payload.Address?.PhysicalAddress.Line1;
            physicalAddress.Line2 = payload.Address?.PhysicalAddress.Line2;
            physicalAddress.State = payload.Address?.PhysicalAddress.State;
            physicalAddress.City = payload.Address?.PhysicalAddress.City;
            physicalAddress.Type = payload.Address?.PhysicalAddress.Type;
            physicalAddress.Zip4 = payload.Address?.PhysicalAddress.Zip4;
            physicalAddress.Zipcode = payload.Address?.PhysicalAddress.Zipcode;
            physicalAddress.ClientId = payload.Demographic.Id;
            physicalAddress.CreatedAt = DateTime.Now;
            await _context.Addresses.AddAsync(physicalAddress);
          }

          physicalAddress.UpdatedAt = DateTime.Now;

        }

        if (payload.Address?.PostalAddress != null)
        {
          var postalAddress = await _context.Addresses.AsNoTracking().FirstOrDefaultAsync(cl => cl.ClientId == payload.Demographic.Id && cl.Type == 2);
          if (postalAddress != null)
          {
            postalAddress.Line1 = payload.Address?.PostalAddress.Line1;
            postalAddress.Line2 = payload.Address?.PostalAddress.Line2;
            postalAddress.State = payload.Address?.PostalAddress.State;
            postalAddress.City = payload.Address?.PostalAddress.City;
            postalAddress.Type = payload.Address?.PostalAddress.Type;
            postalAddress.Zip4 = payload.Address?.PostalAddress.Zip4;
            postalAddress.Zipcode = payload.Address?.PostalAddress.Zipcode;
            _context.Addresses.Update(postalAddress);
          }
          else
          {
            postalAddress = new Addresses();
            postalAddress.Line1 = payload.Address?.PostalAddress.Line1;
            postalAddress.Line2 = payload.Address?.PostalAddress.Line2;
            postalAddress.State = payload.Address?.PostalAddress.State;
            postalAddress.City = payload.Address?.PostalAddress.City;
            postalAddress.Type = payload.Address?.PostalAddress.Type;
            postalAddress.Zip4 = payload.Address?.PostalAddress.Zip4;
            postalAddress.Zipcode = payload.Address?.PostalAddress.Zipcode;
            postalAddress.ClientId = payload.Demographic.Id;
            await _context.Addresses.AddAsync(postalAddress);
          }

          postalAddress.UpdatedAt = DateTime.Now;
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
