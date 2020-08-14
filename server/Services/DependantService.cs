using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using server.Dtos;
using server.Entities;
using WebApi.Entities;
using WebApi.Helpers;

namespace server.Services {
	public interface IDependantService {
		IQueryable<Dependents> GetAll();
		IQueryable<DependentDto> GetAllByClient(int clientId);
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

		public IQueryable<DependentDto> GetAllByClient(int clientId) {
			List<DependentDto> dependents = new List<DependentDto>();
			try {
				dependents.AddRange(this._context.Dependents.Where(x => x.ClientId == clientId) as IEnumerable<DependentDto>);
				dependents.ForEach((item) => {
					item.Name = GetRelationTypes().Where(x => x.Id == item.Relationship)
						.FirstOrDefault() is TypeOfRelationship x ? x.Name : string.Empty;
				});
				return dependents.AsQueryable();
			} catch (System.Exception ex) {
				throw ex;
			}
		}

		public Dependents GetById(int id) {
			throw new System.NotImplementedException();
		}

		public IQueryable<TypeOfRelationship> GetRelationTypes() {
			return _context.TypeOfRelationship.AsQueryable();
		}
	}
}