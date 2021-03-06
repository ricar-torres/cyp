﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApi.Entities;
using WebApi.Helpers;

namespace WebApi.Services
{
  public interface IAgenciesServices
  {
    IQueryable<Agencies> GetAll();
    Agencies GetById(int id);
    Agencies Create(Agencies payload);
    Agencies Update(Agencies payload);
    void Delete(int id);
    Task<Boolean> ChekcName(string criteria);
  }

  public class AgenciesServices : IAgenciesServices
  {
    private readonly DataContext _context;
    private readonly AppSettings _appSettings;

    public AgenciesServices(DataContext context, IOptions<AppSettings> appSettings)
    {
      _context = context;
      _appSettings = appSettings.Value;
    }

    public IQueryable<Agencies> GetAll()
    {
      IQueryable<Agencies> payload = null;

      try
      {

        payload = _context.Agencies.Where(ag => ag.DeletedAt == null).AsQueryable();

      }
      catch (Exception ex)
      {
        throw ex;
      }

      return payload.AsNoTracking();
    }

    public Agencies GetById(int id)
    {
      var res = _context.Agencies.Find(id);
      return res;
    }

    public Agencies Create(Agencies payload)
    {
      try
      {

        payload.CreatedAt = DateTime.Now;
        payload.UpdatedAt = DateTime.Now;
        _context.Agencies.Add(payload);
        _context.SaveChanges();

        return payload;

      }
      catch (Exception ex)
      {

        throw ex;
      }

    }

    public Agencies Update(Agencies payload)
    {
      try
      {

        var item = _context.Agencies.Find(payload.Id);

        if (item == null)
          throw new AppException("Agency not found");

        item.Name = payload.Name;
        item.UpdatedAt = DateTime.Now;

        _context.Agencies.Update(item);
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
      var item = _context.Agencies.Find(id);

      if (item != null)
      {

        item.DeletedAt = DateTime.Now;

        _context.Agencies.Update(item);
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
        var payload = await _context.Agencies.FirstOrDefaultAsync(ag => ag.Name.ToLower().Trim() == criteria);
        if (payload != null)
        {
          return true;
        }
      }
      return false;
    }

  }

}
