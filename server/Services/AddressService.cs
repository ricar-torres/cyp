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
  public interface IAddressService
  {
    Task<List<Addresses>> GetClientAddress(int clientId);
    Task<List<Cities>> GetCities();

    Task<List<States>> GetStates();
    Task<List<Countries>> GetCountries();
  }

  public class AddressService : IAddressService
  {
    private readonly DataContext _context;
    private readonly AppSettings _appSettings;

    public AddressService(DataContext context, IOptions<AppSettings> appSettings)
    {
      _context = context;
      _appSettings = appSettings.Value;
    }

    public async Task<List<Cities>> GetCities()
    {
      try
      {
        var cities = await _context.Cities.Where(ct => ct.DeletedAt == null).ToListAsync();
        return cities;
      }
      catch (Exception ex)
      {
        throw ex;
      }
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

    public async Task<List<Countries>> GetCountries()
    {
      try
      {
        var countries = await _context.Countries.Where(ct => ct.DeletedAt == null).ToListAsync();
        return countries;
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public async Task<List<States>> GetStates()
    {
      try
      {
        var states = await _context.States.Where(ct => ct.DeletedAt == null).ToListAsync();
        return states;
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
  }
}
