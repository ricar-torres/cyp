using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using server.Dtos;
using server.Entities;
using WebApi.Entities;
using WebApi.Helpers;

namespace server.Services {
	public interface IDependantService {
		IQueryable<Dependents> GetAll();
		IQueryable<Dependents> GetAllByClient(int clientId);
		Dependents GetById(int id);
		Dependents Create(Dependents payload);
		IQueryable<TypeOfRelationship> GetRelationTypes();

	}
	public class DependantService : IDependantService {
		private readonly DataContext _context;
		private readonly AppSettings _appSettings;

		public DependantService() { }

		public DependantService(DataContext context, IOptions<AppSettings> appSettings) {
			this._appSettings = appSettings.Value;
			this._context = context;
		}

		public Dependents Create(Dependents payload) {
			throw new System.NotImplementedException();
		}

		public IQueryable<Dependents> GetAll(int cliendId) {
			throw new System.NotImplementedException();

		}

		public IQueryable<Dependents> GetAll() {
			throw new System.NotImplementedException();
		}

		public IQueryable<Dependents> GetAllByClient(int clientId) {
			try {
				return _context.Dependents.Include(x => x.Cover).AsQueryable().AsNoTracking();
			} catch (System.Exception ex) {
				throw ex;
			}
		}

		public Dependents GetById(int id) {
			try {
				return this._context.Dependents.FirstOrDefault(x => x.Id == id);
			} catch (System.Exception ex) {
				throw ex;
			}
		}

		public IQueryable<TypeOfRelationship> GetRelationTypes() {
			return _context.TypeOfRelationship.AsNoTracking();
		}
	}
}