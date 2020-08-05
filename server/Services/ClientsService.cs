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
      var res = _context.Clients.Find(id);
      return res;
    }

    public Clients Create(Clients payload)
    {
      try
      {

        payload.CreatedAt = DateTime.Now;
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

        var item = _context.Clients.Find(payload.Id);

        if (item == null)
          throw new AppException("Agency not found");

        item.Name = payload.Name;
        item.Ssn = payload.Ssn;
        item.Email = payload.Email;
        item.LastName1 = payload.LastName1;
        item.LastName2 = payload.LastName2;
        item.MaritalStatus = payload.MaritalStatus;
        item.BirthDate = payload.BirthDate;
        item.UpdatedAt = DateTime.Now;

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
