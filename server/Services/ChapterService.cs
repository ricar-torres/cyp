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
  public interface IChapterServices
  {
    IQueryable<Chapters> GetAll();
    Chapters GetById(int id);
    Chapters Create(Chapters payload);
    Chapters Update(Chapters payload);
    void Delete(int id);
    Task<Boolean> ChekcName(string criteria);
    Task<List<Chapters>> GetByBonafineId(int id);
  }

  public class ChapterServices : IChapterServices
  {
    private readonly DataContext _context;
    private readonly AppSettings _appSettings;

    public ChapterServices(DataContext context, IOptions<AppSettings> appSettings)
    {
      _context = context;
      _appSettings = appSettings.Value;
    }

    public IQueryable<Chapters> GetAll()
    {
      IQueryable<Chapters> payload = null;

      try
      {

        payload = _context.Chapters.Where(ag => ag.DeletedAt == null).AsQueryable();

      }
      catch (Exception ex)
      {
        throw ex;
      }

      return payload.AsNoTracking();
    }

    public Chapters GetById(int id)
    {
      var res = _context.Chapters.Find(id);
      return res;
    }

    public Chapters Create(Chapters payload)
    {
      try
      {

        payload.CreatedAt = DateTime.Now;
        _context.Chapters.Add(payload);
        _context.SaveChanges();

        return payload;

      }
      catch (Exception ex)
      {

        throw ex;
      }

    }

    public Chapters Update(Chapters payload)
    {
      try
      {

        var item = _context.Chapters.Find(payload.Id);

        if (item == null)
          throw new AppException("Agency not found");

        item.Name = payload.Name;
        item.Quota = payload.Quota;
        item.UpdatedAt = DateTime.Now;

        _context.Chapters.Update(item);
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
      var item = _context.Chapters.Find(id);

      if (item != null)
      {

        item.DeletedAt = DateTime.Now;

        _context.Chapters.Update(item);
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
        var payload = await _context.Chapters.FirstOrDefaultAsync(ag => ag.Name.ToLower().Trim() == criteria);
        if (payload != null)
        {
          return true;
        }
      }
      return false;
    }

    public async Task<List<Chapters>> GetByBonafineId(int id)
    {
      var res = await _context.Chapters.Where(ch => ch.BonaFideId == id && ch.DeletedAt == null).ToListAsync();
      return res;
    }
  }

}
