using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApi.Entities;
using WebApi.Helpers;

namespace server.Services {
	public interface IMultiAssistService {
		IQueryable<HealthPlans> GetAllMultiAssist();
		int Create(MultiAssists payload);
	}
	public class MultiAssistService : IMultiAssistService {
		private readonly DataContext _context;
		private readonly AppSettings _appSettings;

		public MultiAssistService(DataContext context, IOptions<AppSettings> appSettings) {
			_context = context;
			_appSettings = appSettings.Value;
		}

		public int Create(MultiAssists payload) {
			try {
				payload.CreatedAt = DateTime.Now;
				this._context.MultiiAssists.Add(payload);
				this._context.SaveChanges();
				return payload.Id;
			} catch (System.Exception ex) {
				return -1;
			}
		}

		public IQueryable<HealthPlans> GetAllMultiAssist() {
			try {
				var res = this._context.Covers
					.Include(hp => hp.HealthPlan)
					.ThenInclude(_ => _.Covers)
					.ThenInclude(_ => _.MultiAssists)
					.Where(c => c.Type == "ASSIST" || c.Type == "ASSIST-VEH")
					.Select(_ => _.HealthPlan)
					.Distinct()
					.AsNoTracking();
				//  this._context.HealthPlans.Include(_ => _.Covers.Where(c => c.Type == "ASSIST" || c.Type == "ASSIST-VEH")).ThenInclude(_ => _.Type).AsNoTracking();
				return res;
			} catch (System.Exception ex) {

				throw;
			}
		}
	}
}