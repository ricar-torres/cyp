using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using WebApi.Entities;
using WebApi.Helpers;

namespace WebApi.Services {
	public interface IHealthPlanService {
		Task<List<HealthPlans>> GetAll();
		IActionResult GetById(int id);
		HealthPlans Create(HealthPlans item);
		HealthPlans Update(HealthPlans item);
		void Delete(HealthPlans item);

		//addons
		InsuranceAddOns GetAddOnsById(int id);
		InsuranceAddOns CreateAddOns(InsuranceAddOns item);
		InsuranceAddOns UpdateAddOns(InsuranceAddOns item);
		void DeleteAddOns(InsuranceAddOns item);
		ICollection<InsuranceAddOnsRateAge> UploadAddOnsRatesByAge(int InsuranceCompanyId, MemoryStream stream);
		ICollection<InsuranceAddOnsRateAge> UploadAddOnsRatesByAge(int InsuranceCompanyId, int id, MemoryStream stream);
		IActionResult GetAllAddOns(int HealthPlanId);
		IQueryable<InsuranceBenefitType> BenefitType_GetAll();
	}

	public class HealthPlanService : Controller, IHealthPlanService {
		private readonly DataContext _context;
		private readonly AppSettings _appSettings;

		public HealthPlanService(DataContext context, IOptions<AppSettings> appSettings) {
			_context = context;
			_appSettings = appSettings.Value;
		}
		public async Task<List<HealthPlans>> GetAll() {
			var healthPlans = await _context.HealthPlans.Where(hp => hp.DeletedAt == null).ToListAsync();
			return healthPlans;
		}

		public IActionResult GetById(int id) {

			HealthPlans l = null;

			try {

				l = _context.HealthPlans.Include(u => u.Covers).ThenInclude(r => r.AddOns)
					.Include(u => u.InsuranceAddOns).ThenInclude(r => r.RatesByAge).Where(u => u.Id == id).FirstOrDefault();

				if (l == null) {
					throw new AppException("Insurance Company not found");

				}
				var company = new {
					id = l.Id,
						name = l.Name,
						url = l.Url,
						InsurancePlans = from r in l.Covers
					select new {
						r.Id,
							r.Name,
							AddOns = from h in r.AddOns
						select new {
							id = h.InsuranceAddOnsId,
								h.CoverId,
								h.InsuranceAddOns.Name,
								h.InsuranceAddOns.IndividualRate,
								h.InsuranceAddOns.CoverageSingleRate,
								h.InsuranceAddOns.CoverageCoupleRate,
								h.InsuranceAddOns.CoverageFamilyRate,
								h.InsuranceAddOns.MinimumEE,
								h.InsuranceAddOns.TypeCalculate
						}
					}
					//l.InsurancePlans.Select(r => r.AddOns).ToList()
					,
					InsuranceAddOns = from r in l.InsuranceAddOns
					where r.DeletedAt == null
					select new {
						r.Id,
							r.HealthPlanId,
							r.Name,
							r.IndividualRate,
							r.CoverageSingleRate,
							r.CoverageCoupleRate,
							r.CoverageFamilyRate,
							r.MinimumEE,
							r.TypeCalculate,
							RatesByAge = (from h in r.RatesByAge select new { h.Age, h.Rate }).OrderBy(h => h.Age)
					}
				};

				/* from r in l.InsurancePlans
				                             select new { 
				                                 r.Id, r.PlanName, 
				                                 AddOns = String.Join(", ", (from h in r.AddOns 
				                             select new { h.InsuranceAddOns.Name}).Select(h => h.Name).ToArray()) }*/

				return Ok(company);

				//if (item != null)
				//{
				//    if (item.OptionalCovers != null)
				//    { 
				//        item.OptionalCoverAlt  = item.OptionalCovers.Select(r => r.OptionalCoverId).ToArray();
				//    }

				//}

			} catch (Exception ex) {
				throw ex;
			}

		}

		public HealthPlans Create(HealthPlans item) {

			String exception = string.Empty;

			// validation
			//if (item.LoginProviderId == 0)
			//    throw new AppException("LoginProviderId is required");

			//if (!_context.LoginProvider.Any(lp => lp.Id == item.LoginProviderId && lp.DelFlag == false))
			//    throw new AppException("LoginProviderId is invalid");

			//if (string.IsNullOrWhiteSpace(password) && item.LoginProviderId == (int)LoginProviderEnum.Local)
			//    throw new AppException("Password is required");

			//if (string.IsNullOrWhiteSpace(item.UserName))
			//    throw new AppException("UserName is required");

			if (!ValidateRequireField(item, out exception))
				throw new AppException(exception);

			if (_context.HealthPlans.Any(x => x.Name == item.Name))
				throw new AppException("Company Name " + item.Name + " is already exists");

			item.CreatedAt = DateTime.Now;
			_context.HealthPlans.Add(item);
			_context.SaveChanges();

			return item;

		}

		public HealthPlans Update(HealthPlans item) {

			String exception = string.Empty;

			var _HealthPlans = _context.HealthPlans.Find(item.Id);

			if (_HealthPlans == null)
				throw new AppException("Insurance Company not found");

			// validation
			if (!ValidateRequireField(item, out exception))
				throw new AppException(exception);

			if (item.Name.ToLower() != _HealthPlans.Name.ToLower()) {
				// Company Name has changed so check if the new Company Name is already exists
				if (_context.HealthPlans.Any(x => x.Name == item.Name))
					throw new AppException("Company Name " + item.Name + " is already exists");
			}

			_HealthPlans.Name = item.Name;
			_HealthPlans.Url = item.Url;
			_HealthPlans.DeletedAt = item.DeletedAt;
			_HealthPlans.UpdatedAt = DateTime.Now;

			_context.HealthPlans.Update(_HealthPlans);
			_context.SaveChanges();

			return item;

		}

		public void Delete(HealthPlans item) {
			var _HealthPlans = _context.HealthPlans.Find(item.Id);

			if (_HealthPlans != null) {
				_HealthPlans.UpdatedAt = DateTime.Now;
				_HealthPlans.DeletedAt = DateTime.Now;

				_context.HealthPlans.Update(_HealthPlans);
				_context.SaveChanges();
			} else
				throw new AppException("Insurance Company not found");

		}

		public IActionResult GetAllAddOns(int HealthPlanId) {

			// InsuranceAddOns item = null;
			IQueryable<InsuranceAddOns> item = null;

			try {

				item = _context.InsuranceAddOns.Include(u => u.RatesByAge).Where(u => u.HealthPlanId == HealthPlanId && u.DeletedAt == null);

				//if (item != null)
				//{
				//    if (item.OptionalCovers != null)
				//    { 
				//        item.OptionalCoverAlt  = item.OptionalCovers.Select(r => r.OptionalCoverId).ToArray();
				//    }

				//}

			} catch (Exception ex) {
				throw ex;
			}

			return Ok(item);
		}

		public InsuranceAddOns GetAddOnsById(int id) {

			InsuranceAddOns item = null;

			try {

				item = _context.InsuranceAddOns.Include(u => u.RatesByAge).Where(u => u.Id == id).FirstOrDefault();

				//if (item != null)
				//{
				//    if (item.OptionalCovers != null)
				//    { 
				//        item.OptionalCoverAlt  = item.OptionalCovers.Select(r => r.OptionalCoverId).ToArray();
				//    }

				//}

			} catch (Exception ex) {
				throw ex;
			}

			return item;
		}

		public InsuranceAddOns CreateAddOns(InsuranceAddOns item) {

			//var OptionalCover = _context.OptionalCover.Find(item.OptionalCoverId);

			if (string.IsNullOrWhiteSpace(item.Name)) {
				throw new AppException("AddOns name is required!");
				//throw new AppException("LoginProviderId is required");
			}
			if (item.TypeCalculate <= 0) {
				throw new AppException("Type calculate is required!");
				//throw new AppException("LoginProviderId is required");
			}

			if (!_context.HealthPlans.Any(u => u.Id == item.HealthPlanId))
				throw new AppException("Insurance Company not found");

			if (_context.InsuranceAddOns.Any(r => r.HealthPlanId == item.HealthPlanId && r.Name == item.Name)) {

				throw new AppException("AddOns " + item.Name + " is already exists");

			} else {
				item.CreatedAt = DateTime.Now;
				_context.InsuranceAddOns.Add(item);
			}

			_context.SaveChanges();

			return item;

		}

		public InsuranceAddOns UpdateAddOns(InsuranceAddOns item) {

			var _AddOns = _context.InsuranceAddOns.Find(item.Id);

			if (_AddOns == null)
				throw new AppException("AddOns not found");

			if (string.IsNullOrWhiteSpace(item.Name)) {
				throw new AppException("AddOns name is required!");
				//throw new AppException("LoginProviderId is required");
			}
			if (item.TypeCalculate <= 0) {
				throw new AppException("Type calculate is required!");
				//throw new AppException("LoginProviderId is required");
			}

			if (!_context.HealthPlans.Any(u => u.Id == item.HealthPlanId))
				throw new AppException("Insurance Company not found");

			if (item.Name.ToLower() != _AddOns.Name.ToLower()) {
				if (_context.InsuranceAddOns.Any(r => r.HealthPlanId == item.HealthPlanId && r.Name == item.Name)) {
					throw new AppException("AddOns " + item.Name + " is already exists");
				}
			}

			_AddOns.Name = item.Name;
			//_AddOns.Age = item.Age;
			_AddOns.IndividualRate = item.IndividualRate;
			_AddOns.CoverageSingleRate = item.CoverageSingleRate;
			_AddOns.CoverageCoupleRate = item.CoverageCoupleRate;
			_AddOns.CoverageFamilyRate = item.CoverageFamilyRate;
			_AddOns.MinimumEE = item.MinimumEE;
			_AddOns.TypeCalculate = item.TypeCalculate;

			_AddOns.UpdatedAt = DateTime.Now;

			_context.InsuranceAddOns.Update(_AddOns);

			_context.SaveChanges();

			return item;

		}

		public void DeleteAddOns(InsuranceAddOns item) {
			var _AddOns = _context.InsuranceAddOns.Find(item.Id);

			if (_AddOns != null) {

				_AddOns.UpdatedAt = DateTime.Now;
				_AddOns.DeletedAt = DateTime.Now;

				//var EstimateAddOns = _context.InsuranceEstimateAddOns.Where(r => r.InsuranceAddOnsId == _AddOns.Id);

				//if (EstimateAddOns != null)
				//{
				//    _context.InsuranceEstimateAddOns.RemoveRange(EstimateAddOns);
				//    _context.SaveChanges();
				//}

				var PlanAddons = _context.InsurancePlanAddOns.Where(r => r.InsuranceAddOnsId == _AddOns.Id);

				if (PlanAddons != null) {
					_context.InsurancePlanAddOns.RemoveRange(PlanAddons);
					_context.SaveChanges();
				}

				var AddOnsRateAge = _context.InsuranceAddOnsRateAge.Where(r => r.InsuranceAddOnsId == _AddOns.Id);

				if (AddOnsRateAge != null) {
					_context.InsuranceAddOnsRateAge.RemoveRange(AddOnsRateAge);
					_context.SaveChanges();
				}

				_context.InsuranceAddOns.Update(_AddOns);
				_context.SaveChanges();

			} else
				throw new AppException("Insurance Company not found");

		}

		public ICollection<InsuranceAddOnsRateAge> UploadAddOnsRatesByAge(int InsuranceCompanyId, MemoryStream stream) {

			try {

				// ICollection<InsuranceRate> list = null;
				var list = new List<InsuranceAddOnsRateAge>();

				using(var package = new ExcelPackage(stream)) {
					ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
					var rowCount = worksheet.Dimension.Rows;

					for (int row = 2; row <= rowCount; row++) {

						InsuranceAddOns _AddOns = new InsuranceAddOns();
						_AddOns = _context.InsuranceAddOns.Where(u => u.Name == worksheet.Cells[row, 3].Value.ToString()).FirstOrDefault();

						if (_AddOns == null) {

							_AddOns = new InsuranceAddOns();
							_AddOns.Name = worksheet.Cells[row, 3].Value.ToString();
							//_AddOns.Age = item.Age;
							_AddOns.IndividualRate = 0;
							_AddOns.CoverageSingleRate = 0;
							_AddOns.CoverageCoupleRate = 0;
							_AddOns.CoverageFamilyRate = 0;
							_AddOns.MinimumEE = 0;
							_AddOns.TypeCalculate = 4;
							_AddOns.HealthPlanId = InsuranceCompanyId;

							_AddOns.CreatedAt = DateTime.Now;

							_context.InsuranceAddOns.Add(_AddOns);

							_context.SaveChanges();

						}
						int Age = 0;

						string s = worksheet.Cells[row, 1].Value.ToString().Trim();
						int index = s.IndexOf('-');

						if (index == -1) {
							index = s.IndexOf('+');

							if (index == -1) {

								Age = int.Parse(worksheet.Cells[row, 1].Value.ToString().Trim());
								list.Add(new InsuranceAddOnsRateAge {
									InsuranceAddOnsId = _AddOns.Id,
										Age = Age,
										Rate = float.Parse(worksheet.Cells[row, 2].Value == null ? "0.00" : worksheet.Cells[row, 2].Value.ToString().Trim())

								});
							} else {
								string InitialAge = s.Substring(0, index);

								for (int i = int.Parse(InitialAge); i < 100 + 1; i++) {
									list.Add(new InsuranceAddOnsRateAge {
										InsuranceAddOnsId = _AddOns.Id,
											Age = i,
											Rate = float.Parse(worksheet.Cells[row, 2].Value == null ? "0.00" : worksheet.Cells[row, 2].Value.ToString().Trim())

									});
								}

							}
						} else {
							string InitialAge = s.Substring(0, index);
							string FinalAge = s.Substring(index + 1);

							for (int i = int.Parse(InitialAge); i < int.Parse(FinalAge) + 1; i++) {
								list.Add(new InsuranceAddOnsRateAge {
									InsuranceAddOnsId = _AddOns.Id,
										Age = i,
										Rate = float.Parse(worksheet.Cells[row, 2].Value == null ? "0.00" : worksheet.Cells[row, 2].Value.ToString().Trim())

								});
							}

						}

						var ActualRate = _context.InsuranceAddOnsRateAge.Where(r => r.InsuranceAddOnsId == _AddOns.Id).AsQueryable();

						if (ActualRate != null) {
							_context.InsuranceAddOnsRateAge.RemoveRange(ActualRate);
						}
					}

					_context.InsuranceAddOnsRateAge.AddRange(list);
					_context.SaveChanges();

				}

				return list;
			} catch (Exception ex) {
				throw ex;
			}

		}

		public ICollection<InsuranceAddOnsRateAge> UploadAddOnsRatesByAge(int InsuranceCompanyId, int id, MemoryStream stream) {

			try {

				// ICollection<InsuranceRate> list = null;
				var list = new List<InsuranceAddOnsRateAge>();

				using(var package = new ExcelPackage(stream)) {
					ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
					var rowCount = worksheet.Dimension.Rows;

					for (int row = 2; row <= rowCount; row++) {

						InsuranceAddOns _AddOns = new InsuranceAddOns();
						_AddOns = _context.InsuranceAddOns.Find(id);

						if (_AddOns == null || InsuranceCompanyId == 0) {
							throw new AppException("AddOns not found");

						}
						int Age = 0;

						string s = worksheet.Cells[row, 1].Value.ToString().Trim();
						int index = s.IndexOf('-');

						if (index == -1) {
							index = s.IndexOf('+');

							if (index == -1) {

								Age = int.Parse(worksheet.Cells[row, 1].Value.ToString().Trim());
								list.Add(new InsuranceAddOnsRateAge {
									InsuranceAddOnsId = _AddOns.Id,
										Age = Age,
										Rate = float.Parse(worksheet.Cells[row, 2].Value == null ? "0.00" : worksheet.Cells[row, 2].Value.ToString().Trim())

								});
							} else {
								string InitialAge = s.Substring(0, index);

								for (int i = int.Parse(InitialAge); i < 100 + 1; i++) {
									list.Add(new InsuranceAddOnsRateAge {
										InsuranceAddOnsId = _AddOns.Id,
											Age = i,
											Rate = float.Parse(worksheet.Cells[row, 2].Value == null ? "0.00" : worksheet.Cells[row, 2].Value.ToString().Trim())

									});
								}

							}
						} else {
							string InitialAge = s.Substring(0, index);
							string FinalAge = s.Substring(index + 1);

							for (int i = int.Parse(InitialAge); i < int.Parse(FinalAge) + 1; i++) {
								list.Add(new InsuranceAddOnsRateAge {
									InsuranceAddOnsId = _AddOns.Id,
										Age = i,
										Rate = float.Parse(worksheet.Cells[row, 2].Value == null ? "0.00" : worksheet.Cells[row, 2].Value.ToString().Trim())

								});
							}

						}

						var ActualRate = _context.InsuranceAddOnsRateAge.Where(r => r.InsuranceAddOnsId == _AddOns.Id).AsQueryable();

						if (ActualRate != null) {
							_context.InsuranceAddOnsRateAge.RemoveRange(ActualRate);
						}
					}

					_context.InsuranceAddOnsRateAge.AddRange(list);
					_context.SaveChanges();

				}

				return list;
			} catch (Exception ex) {
				throw ex;
			}

		}

		private Boolean ValidateRequireField(HealthPlans item, out String exception)
		{

			exception = string.Empty;

			// validation
			if (string.IsNullOrWhiteSpace(item.Name))
			{
				exception = "Company Name is required";
				return false;
				//throw new AppException("LoginProviderId is required");
			}

			return true;
		}

        public IQueryable<InsuranceBenefitType> BenefitType_GetAll()
        {
            IQueryable<InsuranceBenefitType> item = null;

            try
            {


                item = _context.InsuranceBenefitType.Where(r => r.DeletedAt == null).AsQueryable();

         

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return item;

        }


	}
}