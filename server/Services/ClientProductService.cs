using Microsoft.Extensions.Options;
using WebApi.Entities;
using WebApi.Helpers;

namespace server.Services {
	public interface IClientProductService {
		int Create(ClientProduct payload);
	}
	public class ClientProductService : IClientProductService {
		private readonly DataContext _context;
		private readonly AppSettings _appSettings;

		public ClientProductService(DataContext context, IOptions<AppSettings> appSettings) {
			_context = context;
			_appSettings = appSettings.Value;
		}
		public int Create(ClientProduct payload) {
			try {
				this._context.ClientProduct.Add(payload);
				this._context.SaveChanges();
				return payload.Id;

			} catch (System.Exception ex) {
				return -1;
			}
		}
	}
}