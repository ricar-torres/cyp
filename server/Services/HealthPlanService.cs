using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApi.Entities;
using WebApi.Helpers;

namespace WebApi.Services
{
  public interface IHealthPlanService
  {
    Task<List<HealthPlans>> GetAll();
  }

  public class HealthPlanService : IHealthPlanService
  {
    private readonly DataContext _context;
    private readonly AppSettings _appSettings;

    public HealthPlanService(DataContext context, IOptions<AppSettings> appSettings)
    {
      _context = context;
      _appSettings = appSettings.Value;
    }
    public async Task<List<HealthPlans>> GetAll()
    {
      var healthPlans = await _context.HealthPlans.Where(hp => hp.DeletedAt == null).ToListAsync();
      return healthPlans;
    }
  }
}