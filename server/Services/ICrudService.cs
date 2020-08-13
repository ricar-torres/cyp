using System;
using WebApi.Helpers;

namespace server.Services {
	public interface ICrudService<TEntity> where TEntity : class {
		TEntity Create(TEntity payload);
	}
	public class CrudService<TEntity> : ICrudService<TEntity> where TEntity : class {
		readonly DataContext _dataContext;
		public CrudService(DataContext dataContext) {
			this._dataContext = dataContext;
		}

		public TEntity Create(TEntity payload) {
			try {

				_dataContext.Set<TEntity>().Add(payload);
				_dataContext.SaveChanges();

				return payload;

			} catch (Exception ex) {

				throw ex;
			}
		}
	}
}