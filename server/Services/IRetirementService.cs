using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApi.Entities;
using WebApi.Helpers;

namespace server.Services {
	public interface IRetirementService {
		IQueryable<Retirements> GetAll();
		Retirements GetById(int id);
		Retirements Create(Retirements payload);
		Retirements Update(Retirements payload);
		void Delete(int id);
		bool NameExists(string name);
	}
	public class RetirementService : IRetirementService {
		private readonly DataContext _context;
		private readonly AppSettings _appSettings;
		public RetirementService(DataContext context, IOptions<AppSettings> appSettings) {
			this._appSettings = appSettings.Value;
			this._context = context;
		}
		public Retirements Create(Retirements payload) {
			try {

				payload.CreatedAt = DateTime.Now;
				_context.Retirements.Add(payload);
				_context.SaveChanges();

				return payload;

			} catch (Exception ex) {

				throw ex;
			}
		}

		public void Delete(int id) {
			var item = _context.CommunicationMethods.Find(id);

			if (item != null) {

				item.DeletedAt = DateTime.Now;

				_context.CommunicationMethods.Update(item);
				_context.SaveChanges();
			} else
				throw new AppException("Retirement not found");
		}

		public IQueryable<Retirements> GetAll() {
			IQueryable<Retirements> payload = null;

			try {
				payload = _context.Retirements.Where(ag => ag.DeletedAt == null).AsQueryable();
			} catch (Exception ex) {
				throw ex;
			}
			return payload.AsNoTracking();
		}

		public Retirements GetById(int id) {
			var res = _context.Retirements.Find(id);
			return res;
		}
		public Retirements Update(Retirements payload) {
			try {

				var item = _context.Retirements.Find(payload.Id);

				if (item == null)
					throw new AppException("Communication Method not found");

				item.Name = payload.Name;
				item.Code = payload.Code;
				item.UpdatedAt = DateTime.Now;

				_context.Retirements.Update(item);
				_context.SaveChanges();

				return item;

			} catch (Exception ex) {

				throw ex;
			}
		}
		public bool NameExists(string name) {
			var res = _context.Retirements.Where(c => c.Name.ToLower() == name.ToLower()).FirstOrDefault();
			return res is object;
		}
	}
}