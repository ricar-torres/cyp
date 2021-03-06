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
  public interface IChapterServices
  {
    IQueryable<Chapters> GetAll();
    Chapters GetById(int id);
    Chapters Create(Chapters payload);
    Chapters Update(Chapters payload);
    void Delete(int id);
    Task<Boolean> ChekcName(string name, int? bonafideid);
    Task<List<Chapters>> GetByBonafineId(int id);
    Task<List<Chapters>> getChaptersInBonafideIds(String bonafideIds);
    Task saveClientChapter(ChapterClientDto payload);
    Task<ChapterClient> GetChapterOfClientByBonafideId(int clientId, int bonafideId);
    Task DeleteClientChapter(int clientId, int bonafideId);
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
        payload.UpdatedAt = DateTime.Now;
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
          throw new AppException("Chapter not found");

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


    public async Task<Boolean> ChekcName(string name, int? bonafideid)
    {
      if (!String.IsNullOrEmpty(name) && bonafideid != null)
      {
        name = name.ToLower().Trim();
        var payload = await _context.Chapters.FirstOrDefaultAsync(ag =>
        ag.Name.ToLower().Trim() == name
        &&
        ag.BonaFideId == bonafideid
        &&
        ag.DeletedAt == null
        );
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

    public async Task<List<Chapters>> getChaptersInBonafideIds(String bonafideIds)
    {
      var chapters = await (from ch in _context.Chapters
                            join bn in _context.BonaFides
                            on ch.BonaFideId equals bn.Id
                            where bonafideIds == bn.Id.ToString()
                            orderby bn.Name ascending
                            select ch).Distinct().ToListAsync();
      return chapters;
    }

    public async Task<ChapterClient> GetChapterOfClientByBonafideId(int clientId, int bonafideId)
    {
      var chapterClient = await (from cu in _context.ChapterClient
                                 join ch in _context.Chapters on cu.ChapterId equals ch.Id
                                 join bn in _context.BonaFides on ch.BonaFideId equals bn.Id
                                 where clientId == cu.ClientId && bn.Id == bonafideId
                                 select cu
                    ).FirstOrDefaultAsync();
      return chapterClient;
    }

    public async Task saveClientChapter(ChapterClientDto payload)
    {
      ChapterClient cc = null;
      if (payload.Id != null)
      {
        cc = await _context.ChapterClient.FirstOrDefaultAsync(cc => cc.Id == payload.Id);
        clientChapterBuilder(ref payload, ref cc);
        _context.ChapterClient.Update(cc);
      }
      else
      {
        cc = new ChapterClient();
        clientChapterBuilder(ref payload, ref cc);
        await _context.ChapterClient.AddAsync(cc);
      }
      if (payload.Primary == true)
      {
        var removeFromPrimary = await _context.ChapterClient.Where(ch => ch.ClientId == cc.ClientId && cc.Id != ch.Id).ToListAsync();
        removeFromPrimary.ForEach(el =>
        {
          el.Primary = false;
        });
        _context.ChapterClient.UpdateRange(removeFromPrimary);
      }
      await _context.SaveChangesAsync();
    }

    private static void clientChapterBuilder(ref ChapterClientDto payload, ref ChapterClient cc)
    {
      cc.RegistrationDate = payload.RegistrationDate;
      cc.ChapterId = payload.ChapterId.GetValueOrDefault();
      cc.ClientId = payload.ClientId.GetValueOrDefault();
      cc.Primary = payload.Primary;
      cc.NewRegistration = payload.NewRegistration;
      cc.UpdatedAt = DateTime.Now;
    }

    public async Task DeleteClientChapter(int clientId, int bonafideId)
    {
      var chapterClient = await (from cu in _context.ChapterClient
                                 join ch in _context.Chapters on cu.ChapterId equals ch.Id
                                 join bn in _context.BonaFides on ch.BonaFideId equals bn.Id
                                 where clientId == cu.ClientId && bn.Id == bonafideId
                                 select cu
                    ).FirstOrDefaultAsync();
      _context.ChapterClient.Remove(chapterClient);
      await _context.SaveChangesAsync();
    }
  }

}
