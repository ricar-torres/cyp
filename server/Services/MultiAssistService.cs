using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApi.Entities;
using WebApi.Helpers;

namespace server.Services {
	public interface IMultiAssistService {
		IQueryable<HealthPlans> GetMultiAssistPlans();
		int Create(MultiAssists payload);
		void Update(MultiAssists paylaod);
		MultiAssists GetById(int id);
		IQueryable<MultiAssists> GetAll();
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
				List<Beneficiaries> beneficiariesList;
				List<MultiAssistsVehicle> vehicles;

				payload.CreatedAt = DateTime.Now;
				beneficiariesList = new List<Beneficiaries>(payload.Beneficiaries);
				beneficiariesList.ForEach((item) => {
					item.CreatedAt = DateTime.Now;
					item.Ssn = item.Ssn.TakeLast(4).ToString();
				});
				payload.Beneficiaries = beneficiariesList;

				vehicles = new List<MultiAssistsVehicle>(payload.MultiAssistsVehicle);
				vehicles.ForEach((item) => {
					item.CreatedAt = DateTime.Now;
				});
				payload.MultiAssistsVehicle = vehicles;

				this._context.MultiAssists.Add(payload);
				this._context.SaveChanges();
				return payload.Id;
			} catch (System.Exception ex) {
				return -1;
			}
		}

		public void Update(MultiAssists paylaod) {
			try {
				paylaod.UpdatedAt = DateTime.Now;
				if (_context.MultiAssists.FirstOrDefault(x => x.Id == paylaod.Id) is MultiAssists mas) {
					_context.Entry(mas).CurrentValues.SetValues(paylaod);
					_context.SaveChanges();
				} else {
					throw new AppException("MultiAssist not found");
				}
			} catch (Exception ex) {

				throw ex;
			}
		}

		public IQueryable<HealthPlans> GetMultiAssistPlans() {
			try {
				var res = this._context.Covers
					.Include(hp => hp.HealthPlan)
					.ThenInclude(_ => _.Covers)
					.ThenInclude(_ => _.MultiAssists)
					.Where(c => c.Type == "ASSIST" || c.Type == "ASSIST-VEH")
					.Select(_ => _.HealthPlan)
					.Distinct()
					.AsNoTracking();
				return res;
			} catch (System.Exception ex) {

				throw;
			}
		}

		public IQueryable<MultiAssists> GetAll() {
			try {
				return this._context.MultiAssists.Include(m => m.Cover).AsNoTracking();
			} catch (System.Exception) {
				throw;
			}
		}

		public MultiAssists GetById(int id) {
			MultiAssists res = null;
			try {
				if (_context.MultiAssists
					.Include(c => c.Cover)
					.ThenInclude(h => h.HealthPlan)
					.Include(m => m.Beneficiaries)
					.Include(m => m.MultiAssistsVehicle)
					.FirstOrDefault(ma => ma.Id == id) is MultiAssists ma) {
					res = ma;
				}
				return res;
			} catch (System.Exception ex) {
				throw ex;
			}
		}
	}
}