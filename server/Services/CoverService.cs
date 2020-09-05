using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApi.Entities;
using WebApi.Helpers;
using Newtonsoft.Json;
using ExcelDataReader;
using System.Data;
using OfficeOpenXml;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Services
{
  public interface ICoverService
  {
    Task<List<Covers>> GetAll();
    Task<List<Covers>> GetByPlan(int planId);
        Task<Covers> GetById(int coverId);
        Task<HealthPlans> GetPlanByCover(int coverId);

        Covers Create(Covers item);
        Covers Update(Covers item);
        void Delete(Covers item);

        ICollection<InsuranceRate> UpLoadRate(int InsuranceCompanyId, int PolicyYear, MemoryStream stream);
        InsurancePlanAddOns AddPlanAddOns(InsurancePlanAddOns item);
        void DeletePlanAddOns(int CoverId, int InsuranceAddOnsId);

        InsurancePlanBenefit AddBenefit(InsurancePlanBenefit item);
        void DeleteBenefit(int CoverId, int BenefitTypeId);
        ICollection<InsuranceRate> UploadRatesByAge(int id, int PolicyYear, MemoryStream stream, int userId);
        IQueryable<InsuranceRate> GetRateList(int id);
    }

  public class CoverService : ICoverService
  {
    private readonly DataContext _context;
    private readonly AppSettings _appSettings;

    public CoverService(DataContext context, IOptions<AppSettings> appSettings)
    {
      _context = context;
      _appSettings = appSettings.Value;
    }
    public async Task<List<Covers>> GetAll()
    {
      var healthPlans = await _context.Covers
                    .Include(u => u.BenefitTypes).ThenInclude(r => r.InsuranceBenefitType)
                    .Include(u => u.AddOns).ThenInclude(u => u.InsuranceAddOns)
                    .Where(hp => hp.DeletedAt == null).ToListAsync();
      return healthPlans;
    }
    public async Task<List<Covers>> GetByPlan(int planId)
    {
      var healthPlans = await _context.Covers.Where(cv => cv.DeletedAt == null && cv.HealthPlanId == planId).ToListAsync();
      return healthPlans;
    }
    public async Task<HealthPlans> GetPlanByCover(int coverId)
    {
      var healthPlan =
      await (from hp in _context.HealthPlans
             join cv in _context.Covers on hp.Id equals cv.HealthPlanId
             where cv.Id == coverId
             select hp).FirstOrDefaultAsync();


            return healthPlan;
    }


        public async Task<Covers> GetById(int coverId)
        {
            //var healthPlan =
            //await (from hp in _context.HealthPlans
            //       join cv in _context.Covers on hp.Id equals cv.HealthPlanId
            //       where cv.Id == coverId
            //       select hp).FirstOrDefaultAsync();


            var healthPlan = await _context.Covers
    .Include(u => u.AddOns).ThenInclude(u => u.InsuranceAddOns)
    .Include(u => u.BenefitTypes).ThenInclude(r => r.InsuranceBenefitType)
    .Where(u => u.Id == coverId).FirstOrDefaultAsync();


            return healthPlan;
        }



        public Covers Create(Covers item)
        {

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

            if (_context.Covers.Any(x => x.Name == item.Name && x.HealthPlanId == item.HealthPlanId))
                throw new AppException("Cover Name " + item.Name + " is already exists");


            item.CreatedAt = DateTime.Now;
            _context.Covers.Add(item);
            _context.SaveChanges();

            return item;

        }


        public Covers Update(Covers item)
        {

            String exception = string.Empty;

            var _Covers = _context.Covers.Find(item.Id);


            if (_Covers == null)
                throw new AppException("Cover not found");

            // validation
            if (!ValidateRequireField(item, out exception))
                throw new AppException(exception);

            if (item.Name.ToLower() != _Covers.Name.ToLower())
            {
                // Company Name has changed so check if the new Company Name is already exists
                if (_context.Covers.Any(x => x.Name == item.Name && x.HealthPlanId == item.HealthPlanId))
                    throw new AppException("Cover Name " + item.Name + " is already exists");
            }

            if (string.IsNullOrWhiteSpace(_Covers.Code) || item.Code.ToLower() != _Covers.Code.ToLower())
            {
                // Company Name has changed so check if the new Company Name is already exists
                if (_context.Covers.Any(x => x.Code == item.Code))
                    throw new AppException("Code " + item.Code + " is already exists");
            }

            _Covers.HealthPlanId = item.HealthPlanId;
            _Covers.Code = item.Code;
            _Covers.Name = item.Name;
            _Covers.Sob = item.Sob;
            _Covers.Alianza = item.Alianza;
            _Covers.SobImg = item.SobImg;
            _Covers.Type = item.Type;
            _Covers.UpdatedAt = DateTime.Now;



            _Covers.IndividualRate = item.IndividualRate;
            _Covers.CoverageSingleRate = item.CoverageSingleRate;
            _Covers.CoverageCoupleRate = item.CoverageCoupleRate;
            _Covers.CoverageFamilyRate = item.CoverageFamilyRate;
            _Covers.MinimumEE = item.MinimumEE;
            _Covers.TypeCalculate = item.TypeCalculate;


            _context.Covers.Update(_Covers);
            _context.SaveChanges();

            return item;

        }




        public void Delete(Covers item)
        {
            var _Covers = _context.Covers.Find(item.Id);

            if (_Covers != null)
            {
                _Covers.UpdatedAt = DateTime.Now;
                _Covers.DeletedAt = DateTime.Now;

                _context.Covers.Update(_Covers);
                _context.SaveChanges();
            }
            else
                throw new AppException("Cover not found");

        }


        public IQueryable<InsuranceRate> GetRateList(int id)
        {
            IQueryable<InsuranceRate> item = null;

            try
            {

                item = _context.InsuranceRate
                    .Where(r => r.CoverId == id).AsQueryable();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return item;
        }

        public InsuranceRate AddRate(InsuranceRate item)
        {

            //var OptionalCover = _context.OptionalCover.Find(item.OptionalCoverId);

            if (!_context.Covers.Any(u => u.Id == item.CoverId))
                throw new AppException("Cover not found");

            //if (!_context.InsuranceBenefitType.Any(r => r.Id == item.InsuranceBenefitTypeId && r.DelFlag == false))
            //    throw new AppException("Benefit Type not found");

            if (_context.InsuranceRate.Any(r => r.CoverId == item.CoverId && r.Age == item.Age))
            {

                //_context.InsuranceRate.Update(item);
                throw new AppException("Rate for " + item.Age.ToString() + " year old is already exists");

            }
            else
            {
                _context.InsuranceRate.Add(item);
            }


            _context.SaveChanges();

            return item;

        }

        public ICollection<InsuranceRate> UpLoadRate(int HealthPlanId, int PolicyYear, MemoryStream stream)
        {

            try
            {

                // ICollection<InsuranceRate> list = null;
                var list = new List<InsuranceRate>();

                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {

                        Covers _Covers = new Covers();

                        _Covers = _context.Covers.Where(u => u.Code == worksheet.Cells[row, 6].Value.ToString()).FirstOrDefault();

                        //if (_Covers == null)
                        //{
                        //    //_Covers = new Covers();
                        //    //_Covers.HealthPlanId = HealthPlanId;
                        //    //_Covers.Code = worksheet.Cells[row, 6].Value.ToString();
                        //    //_Covers.Name = worksheet.Cells[row, 7].Value.ToString();
                        //    //_Covers.Sob = worksheet.Cells[row, 1].Value == null ? "" : worksheet.Cells[row, 1].Value.ToString().Trim();
                        //    //_Covers.FederalTIN = worksheet.Cells[row, 2].Value == null ? "" : worksheet.Cells[row, 2].Value.ToString().Trim();
                        //    //_Covers.Type = worksheet.Cells[row, 5].Value == null ? "" : worksheet.Cells[row, 5].Value.ToString().Trim();
                        //    //// _Covers.Tobacco = item.Tobacco;
                        //    //_Covers.Comment = "";
                        //    //_Covers.DelFlag = false;
                        //    //// _Covers.FCreateUserId = GetNameClaim();
                        //    //_Covers.CreatedAt = DateTime.Now;

                        //    //_context.Covers.Add(_Covers);
                        //    //_context.SaveChanges();

                        //}

                        if (_Covers != null)
                         {
                                int Age = 0;


                                string s = worksheet.Cells[row, 9].Value.ToString().Trim();
                                int index = s.IndexOf('-');

                                if (index == -1)
                                {
                                    index = s.IndexOf('+');

                                    if (index == -1)
                                    {


                                        Age = int.Parse(worksheet.Cells[row, 9].Value.ToString().Trim());
                                        list.Add(new InsuranceRate
                                        {
                                            CoverId = _Covers.Id,
                                            Age = Age,
                                            RateEffectiveDate = DateTime.Parse(worksheet.Cells[row, 3].Value == null ? "01/01/1900" : worksheet.Cells[row, 3].Value.ToString().Trim()),
                                            RateExpirationDate = DateTime.Parse(worksheet.Cells[row, 4].Value == null ? "01/01/1900" : worksheet.Cells[row, 4].Value.ToString().Trim()),
                                            IndividualRate = float.Parse(worksheet.Cells[row, 10].Value == null ? "0.00" : worksheet.Cells[row, 10].Value.ToString().Trim()),
                                            //IndividualTobaccoRate = float.Parse(worksheet.Cells[row, 11].Value == null ? "0.00" : worksheet.Cells[row, 11].Value.ToString().Trim()),
                                            PolicyYear = PolicyYear,
                                            CreatedAt = DateTime.Now,
                                            UpdatedAt = DateTime.Now

                                        });
                                    }
                                    else
                                    {
                                        string InitialAge = s.Substring(0, index);

                                        for (int i = int.Parse(InitialAge); i < 100 + 1; i++)
                                        {
                                            list.Add(new InsuranceRate
                                            {
                                                CoverId = _Covers.Id,
                                                Age = i,
                                                RateEffectiveDate = DateTime.Parse(worksheet.Cells[row, 3].Value == null ? "01/01/1900" : worksheet.Cells[row, 3].Value.ToString().Trim()),
                                                RateExpirationDate = DateTime.Parse(worksheet.Cells[row, 4].Value == null ? "01/01/1900" : worksheet.Cells[row, 4].Value.ToString().Trim()),
                                                IndividualRate = float.Parse(worksheet.Cells[row, 10].Value == null ? "0.00" : worksheet.Cells[row, 10].Value.ToString().Trim()),
                                                //IndividualTobaccoRate = float.Parse(worksheet.Cells[row, 11].Value == null ? "0.00" : worksheet.Cells[row, 11].Value.ToString().Trim()),
                                                PolicyYear = PolicyYear,
                                                CreatedAt = DateTime.Now,
                                                UpdatedAt = DateTime.Now

                                            });
                                        }

                                    }
                                }
                                else
                                {
                                    string InitialAge = s.Substring(0, index);
                                    string FinalAge = s.Substring(index + 1);

                                    for (int i = int.Parse(InitialAge); i < int.Parse(FinalAge) + 1; i++)
                                    {
                                        list.Add(new InsuranceRate
                                        {
                                            CoverId = _Covers.Id,
                                            Age = i,
                                            RateEffectiveDate = DateTime.Parse(worksheet.Cells[row, 3].Value == null ? "01/01/1900" : worksheet.Cells[row, 3].Value.ToString().Trim()),
                                            RateExpirationDate = DateTime.Parse(worksheet.Cells[row, 4].Value == null ? "01/01/1900" : worksheet.Cells[row, 4].Value.ToString().Trim()),
                                            IndividualRate = float.Parse(worksheet.Cells[row, 10].Value == null ? "0.00" : worksheet.Cells[row, 10].Value.ToString().Trim()),
                                           // IndividualTobaccoRate = float.Parse(worksheet.Cells[row, 11].Value == null ? "0.00" : worksheet.Cells[row, 11].Value.ToString().Trim()),
                                            PolicyYear = PolicyYear,
                                            CreatedAt = DateTime.Now,
                                            UpdatedAt = DateTime.Now

                                        });
                                    }

                                }

                                var ActualRate = _context.InsuranceRate.Where(r => r.CoverId == _Covers.Id && r.PolicyYear == PolicyYear).AsQueryable();

                                if (ActualRate != null)
                                {
                                    _context.InsuranceRate.RemoveRange(ActualRate);
                                }

                        }
                    }

                    _context.InsuranceRate.AddRange(list);
                    _context.SaveChanges();

                }

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public ICollection<InsuranceRate> UploadRatesByAge(int id, int PolicyYear, MemoryStream stream, int userId)
        {

            try
            {

                // ICollection<InsuranceRate> list = null;
                var list = new List<InsuranceRate>();

                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {


                        Covers _InsurancePlan = new Covers();

                        _InsurancePlan = _context.Covers.Find(id);

                        if (_InsurancePlan == null)
                        {
                            throw new AppException("Insurance Plan not found");

                        }
                        int Age = 0;


                        string s = worksheet.Cells[row, 1].Value.ToString().Trim();
                        int index = s.IndexOf('-');

                        if (index == -1)
                        {
                            index = s.IndexOf('+');

                            if (index == -1)
                            {
                                /*
                                 
                                {
                                    CoverId = _InsurancePlan.Id,
                                    Age = Age,
                                    RateEffectiveDate = DateTime.Parse(worksheet.Cells[row, 3].Value == null ? "01/01/1900" : worksheet.Cells[row, 3].Value.ToString().Trim()),
                                    RateExpirationDate = DateTime.Parse(worksheet.Cells[row, 4].Value == null ? "01/01/1900" : worksheet.Cells[row, 4].Value.ToString().Trim()),
                                    IndividualRate = float.Parse(worksheet.Cells[row, 10].Value == null ? "0.00" : worksheet.Cells[row, 10].Value.ToString().Trim()),
                                    IndividualTobaccoRate = float.Parse(worksheet.Cells[row, 11].Value == null ? "0.00" : worksheet.Cells[row, 11].Value.ToString().Trim()),
                                    PolicyYear = PolicyYear,
                                    CreatedAt = DateTime.Now,
                                    UpdatedAt = DateTime.Now,
                                    FCreateUserId = 0,
                                    FUpdUserId = 0,
                                    DelFlag = false

                                }
                                 */


                                Age = int.Parse(worksheet.Cells[row, 1].Value.ToString().Trim());
                                list.Add(new InsuranceRate
                                {
                                    CoverId = _InsurancePlan.Id,
                                    Age = Age,
                                    RateEffectiveDate = DateTime.Parse($"01/01/{PolicyYear.ToString()}"),
                                    RateExpirationDate = DateTime.Parse($"01/01/{(PolicyYear + 1).ToString()}"),
                                    IndividualRate = float.Parse(worksheet.Cells[row, 2].Value == null ? "0.00" : worksheet.Cells[row, 2].Value.ToString().Trim()),
                                    //IndividualTobaccoRate = float.Parse(worksheet.Cells[row, 2].Value == null ? "0.00" : worksheet.Cells[row, 2].Value.ToString().Trim()),
                                    PolicyYear = PolicyYear,
                                    CreatedAt = DateTime.Now,
                                    UpdatedAt = DateTime.Now,
                                    DeletedAt = null
                                    //Rate = float.Parse(worksheet.Cells[row, 2].Value == null ? "0.00" : worksheet.Cells[row, 2].Value.ToString().Trim())

                                });
                            }
                            else
                            {
                                string InitialAge = s.Substring(0, index);

                                for (int i = int.Parse(InitialAge); i < 100 + 1; i++)
                                {
                                    list.Add(new InsuranceRate
                                    {
                                        CoverId = _InsurancePlan.Id,
                                        Age = i,
                                        RateEffectiveDate = DateTime.Parse($"01/01/{PolicyYear.ToString()}"),
                                        RateExpirationDate = DateTime.Parse($"01/01/{(PolicyYear + 1).ToString()}"),
                                        IndividualRate = float.Parse(worksheet.Cells[row, 2].Value == null ? "0.00" : worksheet.Cells[row, 2].Value.ToString().Trim()),
                                        //IndividualTobaccoRate = float.Parse(worksheet.Cells[row, 2].Value == null ? "0.00" : worksheet.Cells[row, 2].Value.ToString().Trim()),
                                        PolicyYear = PolicyYear,
                                        CreatedAt = DateTime.Now,
                                        UpdatedAt = DateTime.Now,
                                        DeletedAt = null

                                    });
                                }

                            }
                        }
                        else
                        {
                            string InitialAge = s.Substring(0, index);
                            string FinalAge = s.Substring(index + 1);

                            for (int i = int.Parse(InitialAge); i < int.Parse(FinalAge) + 1; i++)
                            {
                                list.Add(new InsuranceRate
                                {
                                    CoverId = _InsurancePlan.Id,
                                    Age = i,
                                    RateEffectiveDate = DateTime.Parse($"01/01/{PolicyYear.ToString()}"),
                                    RateExpirationDate = DateTime.Parse($"01/01/{(PolicyYear + 1).ToString()}"),
                                    IndividualRate = float.Parse(worksheet.Cells[row, 2].Value == null ? "0.00" : worksheet.Cells[row, 2].Value.ToString().Trim()),
                                    //IndividualTobaccoRate = float.Parse(worksheet.Cells[row, 2].Value == null ? "0.00" : worksheet.Cells[row, 2].Value.ToString().Trim()),
                                    PolicyYear = PolicyYear,
                                    CreatedAt = DateTime.Now,
                                    UpdatedAt = DateTime.Now,
                                    DeletedAt = null

                                });
                            }

                        }

                        var ActualRate = _context.InsuranceRate.Where(r => r.CoverId == _InsurancePlan.Id).AsQueryable();

                        if (ActualRate != null)
                        {
                            _context.InsuranceRate.RemoveRange(ActualRate);
                        }
                    }

                    _context.InsuranceRate.AddRange(list.Distinct());
                    _context.SaveChanges();

                }

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public InsurancePlanBenefit AddBenefit(InsurancePlanBenefit item)
        {

            //var OptionalCover = _context.OptionalCover.Find(item.OptionalCoverId);

            if (!_context.Covers.Any(u => u.Id == item.CoverId))
                throw new AppException("Insurance Plan not found");

            if (!_context.InsuranceBenefitType.Any(r => r.Id == item.InsuranceBenefitTypeId && r.DeletedAt == null))
                throw new AppException("Benefit Type not found");

            if (_context.InsurancePlanBenefit.Any(r => r.CoverId == item.CoverId && r.InsuranceBenefitTypeId == item.InsuranceBenefitTypeId))
            {

                _context.InsurancePlanBenefit.Update(item);
                //throw new AppException("Optional Cover " + OptionalCover.Name  + " is already exists");

            }
            else
            {
                _context.InsurancePlanBenefit.Add(item);
            }


            _context.SaveChanges();

            return item;

        }



        public InsurancePlanAddOns AddPlanAddOns(InsurancePlanAddOns item)
        {

            //var OptionalCover = _context.OptionalCover.Find(item.OptionalCoverId);

            if (!_context.Covers.Any(u => u.Id == item.CoverId))
                throw new AppException("Insurance Plan not found");

            if (!_context.InsuranceAddOns.Any(r => r.Id == item.InsuranceAddOnsId && r.DeletedAt == null))
                throw new AppException("AddOns not found");

            if (_context.InsurancePlanAddOns.Any(r => r.CoverId == item.CoverId && r.InsuranceAddOnsId == item.InsuranceAddOnsId))
            {

                //_context.InsurancePlanAddOns.Update(item);
                throw new AppException("Plan AddOns is already exists");

            }
            else
            {
                _context.InsurancePlanAddOns.Add(item);
            }


            _context.SaveChanges();

            return item;

        }

        public void DeletePlanAddOns(int CoversId, int InsuranceAddOnsId)
        {

            var item = _context.InsurancePlanAddOns.Find(CoversId, InsuranceAddOnsId);

            if (item != null)
            {
                _context.InsurancePlanAddOns.Remove(item);
                _context.SaveChanges();
            }

        }


        public void DeleteBenefit(int CoversId, int BenefitTypeId)
        {

            var item = _context.InsurancePlanBenefit.Find(CoversId, BenefitTypeId);

            if (item != null)
            {
                _context.InsurancePlanBenefit.Remove(item);
                _context.SaveChanges();
            }

        }

        private Boolean ValidateRequireField(Covers item, out String exception)
        {

            exception = string.Empty;

            // validation
            if (string.IsNullOrWhiteSpace(item.Name))
            {
                exception = "Cover Name is required";
                return false;
                //throw new AppException("LoginProviderId is required");
            }

            // validation
            //else if (string.IsNullOrWhiteSpace(item.Code))
            //{
            //    exception = "Code is required";
            //    return false;
            //    //throw new AppException("LoginProviderId is required");
            //}

            return true;

        }


    }
}