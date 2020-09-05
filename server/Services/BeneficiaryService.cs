using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using WebApi.Entities;
using WebApi.Helpers;

namespace server.Services {
	public interface IBeneficiaryService {
		void Create(List<Beneficiaries> payload);
	}
	public class BeneficiaryService : IBeneficiaryService {
		private readonly DataContext _context;
		private readonly AppSettings _appSettings;

		public BeneficiaryService(DataContext context, IOptions<AppSettings> appSettings) {
			_context = context;
			_appSettings = appSettings.Value;
		}
		public void Create(List<Beneficiaries> payload) {
			try {
				payload.ForEach((item) => {
					item.CreatedAt = DateTime.Now;
				});
				this._context.Beneficiaries.AddRange(payload);
				this._context.SaveChanges();
			} catch (System.Exception ex) { }
		}
	}
}