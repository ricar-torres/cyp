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
  public interface IClientService
  {
    List<Clients> GetAll();
    Clients GetById(int id);
    Clients Create(Clients payload);
    Clients Update(Clients payload);
    void Delete(int id);
    Task<Boolean> ChekcName(string criteria);
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
      var physicalAddress = _context.Addresses.FirstOrDefault(a => a.ClientId == res.Id && a.Type == 1);
      var postalAddress = _context.Addresses.FirstOrDefault(a => a.ClientId == res.Id && a.Type == 2);
      res.PhysicalAddress = physicalAddress;
      res.PostalAddress = postalAddress;
      return res;
    }

    public Clients Create(Clients payload)
    {
      try
      {

        payload.CreatedAt = DateTime.Now;
        payload.UpdatedAt = DateTime.Now;
        _context.Clients.Add(payload);
        _context.SaveChanges();

        return payload;

      }
      catch (Exception ex)
      {

        throw ex;
      }

    }

    public Clients Update(Clients payload)
    {
      try
      {

        var item = _context.Clients.FirstOrDefault(c => c.Id == payload.Id);
        var physicalAddress = _context.Addresses.FirstOrDefault(p => p.ClientId == item.Id && p.Type == 1);
        var postalAddress = _context.Addresses.FirstOrDefault(p => p.ClientId == item.Id && p.Type == 2);

        if (item == null)
          throw new AppException("Client not found");

        item.Name = payload.Name;
        item.Initial = payload.Initial;
        item.Ssn = payload.Ssn;
        item.Email = payload.Email;
        item.LastName1 = payload.LastName1;
        item.LastName2 = payload.LastName2;
        item.MaritalStatus = payload.MaritalStatus;
        item.BirthDate = payload.BirthDate;
        item.Gender = payload.Gender;
        item.Phone1 = payload.Phone1;
        item.Phone2 = payload.Phone2;

        if (payload.PhysicalAddress != null)
        {
          if (physicalAddress != null)
          {
            physicalAddress.City = payload.PhysicalAddress.City;
            physicalAddress.State = payload.PhysicalAddress.State;
            physicalAddress.Line1 = payload.PhysicalAddress.Line1;
            physicalAddress.Line2 = payload.PhysicalAddress.Line2;
            physicalAddress.Zipcode = payload.PhysicalAddress.Zipcode;
            physicalAddress.Zip4 = payload.PhysicalAddress.Zip4;
            physicalAddress.UpdatedAt = DateTime.Now;
          }
          else
          {
            _context.Addresses.Add(payload.PhysicalAddress);
          }
        }

        if (payload.PostalAddress != null)
        {
          if (postalAddress != null)
          {
            postalAddress.City = payload.PostalAddress.City;
            postalAddress.State = payload.PostalAddress.State;
            postalAddress.Line1 = payload.PostalAddress.Line1;
            postalAddress.Line2 = payload.PostalAddress.Line2;
            postalAddress.Zipcode = payload.PostalAddress.Zipcode;
            postalAddress.Zip4 = payload.PostalAddress.Zip4;
            postalAddress.UpdatedAt = DateTime.Now;
          }
          else
          {
            _context.Addresses.Add(payload.PostalAddress);
          }
        }

        item.UpdatedAt = DateTime.Now;



        _context.Addresses.Update(physicalAddress);
        _context.Addresses.Update(postalAddress);
        _context.Clients.Update(item);
        _context.SaveChanges();
        return item;
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

  }

}
