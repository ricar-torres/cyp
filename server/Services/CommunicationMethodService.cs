using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApi.Entities;
using WebApi.Helpers;

namespace server.Services {
	public interface ICommunicationMethodService {
		IQueryable<CommunicationMethods> GetAll();
		CommunicationMethods GetById(int id);
		CommunicationMethods Create(CommunicationMethods payload);
		CommunicationMethods Update(CommunicationMethods payload);
		void Delete(int id);
		bool NameExists(string name);
	}
	public class CommunicationMethodService : ICommunicationMethodService {
		private readonly DataContext _context;
		private readonly AppSettings _appSettings;
		public CommunicationMethodService(DataContext context, IOptions<AppSettings> appSettings) {
			this._appSettings = appSettings.Value;
			this._context = context;
		}

		public CommunicationMethods Create(CommunicationMethods payload) {
			try {

				payload.CreatedAt = DateTime.Now;
				_context.CommunicationMethods.Add(payload);
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
				throw new AppException("Communication Methods not found");
		}

		public IQueryable<CommunicationMethods> GetAll() {
			IQueryable<CommunicationMethods> payload = null;

			try {
				payload = _context.CommunicationMethods.Where(ag => ag.DeletedAt == null).AsQueryable();
			} catch (Exception ex) {
				throw ex;
			}
			return payload.AsNoTracking();
		}

		public CommunicationMethods GetById(int id) {
			var res = _context.CommunicationMethods.Find(id);
			return res;
		}
		public CommunicationMethods Update(CommunicationMethods payload) {
			try {

				var item = _context.CommunicationMethods.Find(payload.Id);

				if (item == null)
					throw new AppException("Communication Method not found");

				item.Name = payload.Name;
				item.UpdatedAt = DateTime.Now;

				_context.CommunicationMethods.Update(item);
				_context.SaveChanges();

				return item;

			} catch (Exception ex) {

				throw ex;
			}
		}
		public bool NameExists(string name) {
			var res = _context.CommunicationMethods.Where(c => c.Name.ToLower() == name.ToLower()).FirstOrDefault();
			return res is object;
		}
	}
}