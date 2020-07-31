using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApi.Entities;
using WebApi.Helpers;

namespace WebApi.Services {

	public interface ICampaignsService {
		IQueryable<Campaigns> GetAll();
		Campaigns GetById(int id);
		Campaigns Create(Campaigns payload);
		Campaigns Update(Campaigns payload);
		void Delete(int id);
		bool NameExists(string name);
	}

	public class CampaignsService : ICampaignsService {
		private readonly DataContext _context;
		private readonly AppSettings _appSettings;

		public CampaignsService(DataContext context, IOptions<AppSettings> appSettings) {
			_context = context;
			_appSettings = appSettings.Value;
		}

		public IQueryable<Campaigns> GetAll() {
			IQueryable<Campaigns> payload = null;

			try {

				payload = _context.Campaigns.Where(ag => ag.DeletedAt == null).AsQueryable();

			} catch (Exception ex) {
				throw ex;
			}

			return payload.AsNoTracking();
		}

		public Campaigns GetById(int id) {
			var res = _context.Campaigns.Find(id);
			return res;
		}
		public bool NameExists(string name) {
			var res = _context.Campaigns.Where(c => c.Name.ToLower() == name.ToLower()).FirstOrDefault();
			return res is object;
		}

		public Campaigns Create(Campaigns payload) {
			try {

				payload.CreatedAt = DateTime.Now;
				_context.Campaigns.Add(payload);
				_context.SaveChanges();

				return payload;

			} catch (Exception ex) {

				throw ex;
			}
		}

		public Campaigns Update(Campaigns payload) {
			try {

				var item = _context.Campaigns.Find(payload.Id);

				if (item == null)
					throw new AppException("Agency not found");

				item.Name = payload.Name;
				item.UpdatedAt = DateTime.Now;
				item.Origin = payload.Origin;

				_context.Campaigns.Update(item);
				_context.SaveChanges();

				return item;

			} catch (Exception ex) {

				throw ex;
			}
		}

		public void Delete(int id) {
			var item = _context.Campaigns.Find(id);

			if (item != null) {

				item.DeletedAt = DateTime.Now;

				_context.Campaigns.Update(item);
				_context.SaveChanges();
			} else
				throw new AppException("Agency not found");

		}

	}
}