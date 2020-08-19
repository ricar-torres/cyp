using System;
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
		IQueryable<Covers> GetCovers();
		IQueryable<HealthPlans> GetHealthPlans();
		Dependents Update(Dependents dependent);

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
			try {
				payload.CreatedAt = DateTime.Now;
				this._context.Dependents.Add(payload as Dependents);
				this._context.SaveChanges();
			} catch (System.Exception ex) {

				throw ex;
			}

			return payload;
		}

		public IQueryable<Dependents> GetAll(int cliendId) {
			throw new System.NotImplementedException();

		}

		public IQueryable<Dependents> GetAll() {
			throw new System.NotImplementedException();
		}

		public IQueryable<Dependents> GetAllByClient(int clientId) {
			try {
				return _context.Dependents
					.Include(x => x.Cover)
					.ThenInclude(x => x.HealthPlan).AsQueryable().AsNoTracking();
			} catch (System.Exception ex) {
				throw ex;
			}
		}

		public Dependents GetById(int id) {
			try {
				return this._context.Dependents.Include(x => x.Cover).ThenInclude(x => x.HealthPlan).FirstOrDefault(x => x.Id == id);
			} catch (System.Exception ex) {
				throw ex;
			}
		}

		public IQueryable<Covers> GetCovers() {
			return _context.Covers.AsNoTracking();
		}

		public IQueryable<HealthPlans> GetHealthPlans() {
			return _context.HealthPlans.AsNoTracking();
		}

		public IQueryable<TypeOfRelationship> GetRelationTypes() {
			return _context.TypeOfRelationship.AsNoTracking();
		}

		public Dependents Update(Dependents dependent) {
			try {

				var item = _context.Dependents.Find(dependent.Id);

				if (item == null)
					throw new AppException("Dependent not found");

				item = dependent;
				item.UpdatedAt = DateTime.Now;

				_context.Dependents.Update(item);
				_context.SaveChanges();

				return item;

			} catch (Exception ex) {

				throw ex;
			}
		}
	}
}