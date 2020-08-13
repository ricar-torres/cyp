using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
		IQueryable<CallReasons> GetCallReasons();
		string CreateConfirmationCode();
		//IQueryable<IEnumerable<string>> GetClientCalls(int clientId);

	}
	public class DocumentationCallService : IDocumentationCallService {
		private readonly DataContext _context;
		private readonly AppSettings _appSettings;
		public DocumentationCallService(DataContext context, IOptions<AppSettings> appSettings) {
			this._appSettings = appSettings.Value;
			this._context = context;
		}

		public DocumentationCallService() { }

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

		public IQueryable<CallReasons> GetCallReasons() {
			return _context.CallReasons.AsNoTracking();
		}
		public string CreateConfirmationCode() {
			bool exist = true;
			Random random;
			int pin;
			string code = string.Empty;
			DateTime date;

			date = DateTime.Now;
			random = new Random(date.TimeOfDay.Milliseconds);
			do {
				try {
					pin = random.Next(1000, 9999);
					code = date.ToString("yy") +
						date.ToString("MM") +
						date.ToString("dd") +
						date.ToString("hh") +
						date.ToString("mm");

				} catch (System.Exception ex) {
					throw ex;
				}
			} while (exist);

			return code;
		}

		// public IQueryable<IEnumerable<string>> GetClientCalls(int clientId) {
		// 	IQueryable<IEnumerable<string>> result;
		// 	try {
		// 		result =
		// 		_context.ClientUser.Select(x=>x.ConfirmationNumber).AsQueryable();
		// 		// _context.ClientUser
		// 		// 	.GroupBy(x => x.ConfirmationNumber)
		// 		// 	.Select(x => x.OrderBy(a => a.Id))
		// 		// 	.SelectMany(x=>x).ToList();
		// 	} catch (Exception ex) {
		// 		throw ex;
		// 	}
		// 	return result.AsNoTracking();
		// }

	}
}