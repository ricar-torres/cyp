using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApi.Entities;
using WebApi.Helpers;

namespace WebApi.Services
{
  public interface IQualifyingEventsSerivie
  {
    IQueryable<QualifyingEvents> GetAll();
    QualifyingEvents GetById(int id);
    QualifyingEvents Create(QualifyingEvents payload);
    QualifyingEvents Update(QualifyingEvents payload);
    void Delete(int id);
    Task<Boolean> ChekcName(string criteria);
  }

  public class QualifyingEventsSerivie : IQualifyingEventsSerivie
  {
    private readonly DataContext _context;
    private readonly AppSettings _appSettings;

    public QualifyingEventsSerivie(DataContext context, IOptions<AppSettings> appSettings)
    {
      _context = context;
      _appSettings = appSettings.Value;
    }

    public IQueryable<QualifyingEvents> GetAll()
    {
      IQueryable<QualifyingEvents> payload = null;

      try
      {

        payload = _context.QualifyingEvents.Where(ag => ag.DeletedAt == null).AsQueryable();

      }
      catch (Exception ex)
      {
        throw ex;
      }

      return payload.AsNoTracking();
    }

    public QualifyingEvents GetById(int id)
    {
      var res = _context.QualifyingEvents.Find(id);
      return res;
    }

    public QualifyingEvents Create(QualifyingEvents payload)
    {
      try
      {

        payload.CreatedAt = DateTime.Now;
        _context.QualifyingEvents.Add(payload);
        _context.SaveChanges();

        return payload;

      }
      catch (Exception ex)
      {

        throw ex;
      }

    }

    public QualifyingEvents Update(QualifyingEvents payload)
    {
      try
      {

        var item = _context.QualifyingEvents.Find(payload.Id);

        if (item == null)
          throw new AppException("Agency not found");

        item.Name = payload.Name;
        item.UpdatedAt = DateTime.Now;

        _context.QualifyingEvents.Update(item);
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
      var item = _context.QualifyingEvents.Find(id);

      if (item != null)
      {

        item.DeletedAt = DateTime.Now;

        _context.QualifyingEvents.Update(item);
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
        var payload = await _context.QualifyingEvents.FirstOrDefaultAsync(ag => ag.Name.ToLower().Trim() == criteria);
        if (payload != null)
        {
          return true;
        }
      }
      return false;
    }

  }

}
