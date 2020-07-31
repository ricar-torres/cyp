using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Novell.Directory.Ldap;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebApi.Dtos;
using WebApi.Entities;
using WebApi.Entities.Identity;
using WebApi.Enums;
using WebApi.Helpers;

namespace WebApi.Services
{

  public interface IBonaFidesServices
  {
    IQueryable<BonaFides> GetAll();
    BonaFides GetById(int id);
    BonaFides Create(BonaFides payload);
    BonaFides Update(BonaFides payload);
    void Delete(int id);
    Task<Boolean> ChekcName(string criteria);
    Task<Boolean> ChekcEmail(string email);
  }

  public class BonaFidesServices : Controller, IBonaFidesServices
  {

    private readonly DataContext _context;
    private readonly AppSettings _appSettings;

    public BonaFidesServices(DataContext context, IOptions<AppSettings> appSettings)
    {
      _context = context;
      _appSettings = appSettings.Value;
    }
    public BonaFides Create(BonaFides payload)
    {
      try
      {

        payload.CreatedAt = DateTime.Now;
        _context.BonaFides.Add(payload);
        _context.SaveChanges();

        return payload;

      }
      catch (Exception ex)
      {

        throw ex;
      }
    }

    public void Delete(int id)
    {
      var item = _context.BonaFides.Find(id);

      if (item != null)
      {

        item.DeletedAt = DateTime.Now;

        _context.BonaFides.Update(item);
        _context.SaveChanges();
      }
      else
        throw new AppException("Agency not found");
    }

    public IQueryable<BonaFides> GetAll()
    {
      IQueryable<BonaFides> payload = null;

      try
      {

        payload = _context.BonaFides.Where(ag => ag.DeletedAt == null).AsQueryable();

      }
      catch (Exception ex)
      {
        throw ex;
      }

      return payload.AsNoTracking();
    }

    public BonaFides GetById(int id)
    {
      var res = _context.BonaFides.Find(id);
      return res;
    }

    public BonaFides Update(BonaFides payload)
    {
      try
      {

        var item = _context.BonaFides.Find(payload.Id);

        if (item == null)
          throw new AppException("Agency not found");

        item.Name = payload.Name;
        item.Code = payload.Code;
        item.Siglas = payload.Siglas;
        item.Phone = payload.Phone;
        item.Email = payload.Email;
        item.Benefits = payload.Benefits;
        item.Disclaimer = payload.Disclaimer;
        item.UpdatedAt = DateTime.Now;

        _context.BonaFides.Update(item);
        _context.SaveChanges();

        return item;

      }
      catch (Exception ex)
      {

        throw ex;
      }
    }

    public async Task<Boolean> ChekcName(string criteria)
    {
      System.Diagnostics.Debug.Write("checkName");
      if (!String.IsNullOrEmpty(criteria))
      {
        criteria = criteria.ToLower().Trim();
        var payload = await _context.BonaFides.FirstOrDefaultAsync(bf => bf.Name.ToLower().Trim() == criteria);
        if (payload != null)
        {
          return true;
        }
      }
      return false;
    }

    public async Task<Boolean> ChekcEmail(string email)
    {
      System.Diagnostics.Debug.Write("checkName");
      if (!String.IsNullOrEmpty(email))
      {
        email = email.ToLower().Trim();
        var payload = await _context.BonaFides.FirstOrDefaultAsync(bf => bf.Email.ToLower().Trim() == email);
        if (payload != null)
        {
          return true;
        }
      }
      return false;
    }
  }
}