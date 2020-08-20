using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApi.Entities;
using WebApi.Helpers;

namespace WebApi.Services
{
  public interface ICoverService
  {
    Task<List<Covers>> GetAll();
    Task<List<Covers>> GetByPlan(int planId);
    Task<HealthPlans> GetPlanByCover(int coverId);
  }

  public class CoverService : ICoverService
  {
    private readonly DataContext _context;
    private readonly AppSettings _appSettings;

    public CoverService(DataContext context, IOptions<AppSettings> appSettings)
    {
      _context = context;
      _appSettings = appSettings.Value;
    }
    public async Task<List<Covers>> GetAll()
    {
      var healthPlans = await _context.Covers.Where(hp => hp.DeletedAt == null).ToListAsync();
      return healthPlans;
    }
    public async Task<List<Covers>> GetByPlan(int planId)
    {
      var healthPlans = await _context.Covers.Where(cv => cv.DeletedAt == null && cv.HealthPlanId == planId).ToListAsync();
      return healthPlans;
    }
    public async Task<HealthPlans> GetPlanByCover(int coverId)
    {
      var healthPlan =
      await (from hp in _context.HealthPlans
             join cv in _context.Covers on hp.Id equals cv.HealthPlanId
             where cv.Id == coverId
             select hp).FirstOrDefaultAsync();
      return healthPlan;
    }
  }
}