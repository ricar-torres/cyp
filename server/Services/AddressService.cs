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
  public interface IAddressService
  {
    Task<List<Addresses>> GetClientAddress(int clientId);
    Task<List<Addresses>> UpdateClientAddress(List<Addresses> addresses, int cientId);
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

    public async Task<List<Addresses>> UpdateClientAddress(List<Addresses> addresses, int clientId)
    {
      try
      {
        var physicalAddress = await _context.Addresses.FirstOrDefaultAsync(addr => addr.ClientId == clientId && addr.Type == 1);
        var postalAddress = await _context.Addresses.FirstOrDefaultAsync(addr => addr.ClientId == clientId && addr.Type == 2);

        var UpdatePhysicalAddress = addresses.FirstOrDefault(addr => addr.Type == 1);
        var UpdatePostalAddress = addresses.FirstOrDefault(addr => addr.Type == 2);

        physicalAddress = UpdatePhysicalAddress != null ? UpdatePhysicalAddress : physicalAddress;
        postalAddress = UpdatePostalAddress != null ? UpdatePostalAddress : postalAddress;

        var resolve = new List<Addresses>
        {physicalAddress,postalAddress};

        _context.Addresses.Update(physicalAddress);
        _context.Addresses.Update(postalAddress);
        await _context.SaveChangesAsync();

        return resolve;

      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
  }
}
