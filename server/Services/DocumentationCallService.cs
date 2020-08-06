using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApi.Entities;
using WebApi.Helpers;

namespace server.Services {
	public interface IDocumentationCallService {
		IQueryable<ClientUser> GetAll();
		IQueryable<ClientUser> GetAllByClient(int clientId);
		ClientUser GetById(int id);
		ClientUser Create(ClientUser payload);

	}
	public class DocumentationCallService : IDocumentationCallService {
		private readonly DataContext _context;
		private readonly AppSettings _appSettings;
		public DocumentationCallService(DataContext context, IOptions<AppSettings> appSettings) {
			this._appSettings = appSettings.Value;
			this._context = context;
		}

		public ClientUser Create(ClientUser payload) {
			try {

				payload.CreatedAt = DateTime.Now;
				_context.ClientUser.Add(payload);
				_context.SaveChanges();

				return payload;

			} catch (Exception ex) {

				throw ex;
			}
		}

		public IQueryable<ClientUser> GetAll() {
			IQueryable<ClientUser> result;
			try {
				result = _context.ClientUser.AsQueryable();
			} catch (Exception ex) {
				throw ex;
			}
			return result.AsNoTracking();
		}

		public IQueryable<ClientUser> GetAllByClient(int clientId) {
			IQueryable<ClientUser> result;
			try {
				result = _context.ClientUser.AsQueryable();
			} catch (Exception ex) {
				throw ex;
			}
			return result.AsNoTracking();
		}
		public ClientUser GetById(int id) {
			var res = _context.ClientUser.Find(id);
			return res;
		}
	}
}