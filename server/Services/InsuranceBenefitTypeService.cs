using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApi.Entities;
using WebApi.Helpers;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace WebApi.Services
{


    public interface IInsuranceBenefitTypeService
    {
        IQueryable<InsuranceBenefitType> GetAll();
        InsuranceBenefitType GetById(int id);
        InsuranceBenefitType Create(InsuranceBenefitType item);
        InsuranceBenefitType Update(InsuranceBenefitType item);
        void Delete(InsuranceBenefitType item);
    }
    public class InsuranceBenefitTypeService : IInsuranceBenefitTypeService
    {

        private DataContext _context;
        private readonly AppSettings _appSettings;

        public InsuranceBenefitTypeService(DataContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        public IQueryable<InsuranceBenefitType> GetAll()
        {
            IQueryable<InsuranceBenefitType> item = null;

            try
            {


                item = _context.InsuranceBenefitType.Where(r => r.DeletedAt == null).AsQueryable();

                //if (filter.Id != null)
                //{
                //    item = _context.InsuranceBenefitType.Where(r => filter.Id.Any(f => f == r.Id)).AsQueryable();
                //}
                //else
                //{

                //    if (filter.DelFlag != null && string.IsNullOrWhiteSpace(filter.Name))
                //    {
                //        item = _context.InsuranceBenefitType.Where(r => r.DelFlag == filter.DelFlag).AsQueryable();
                //    }

                //    if (!string.IsNullOrWhiteSpace(filter.Name) && filter.DelFlag != null)
                //    {
                //        item = _context.InsuranceBenefitType.Where(r => EF.Functions.Like(r.Name, string.Format("%{0}%", filter.Name)) && r.DelFlag == filter.DelFlag).AsQueryable();
                //    }

                //    if (string.IsNullOrWhiteSpace(filter.Name) && filter.DelFlag == null)
                //    {
                //        item = _context.InsuranceBenefitType.AsQueryable();
                //    }

                //}

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return item;

        }


        public InsuranceBenefitType GetById(int id)
        {

            InsuranceBenefitType item = null;

            try
            {

                item = _context.InsuranceBenefitType.Where(u => u.Id == id).FirstOrDefault();


            }
            catch (Exception ex)
            {
                throw ex;
            }


            return item;
        }


        public InsuranceBenefitType Create(InsuranceBenefitType item)
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

            if (_context.InsuranceBenefitType.Any(x => x.BenefitType == item.BenefitType))
                throw new AppException("Benefit Type Name" + item.BenefitType + " is already exists");


            item.CreatedAt = DateTime.Now;
            item.UpdatedAt = DateTime.Now;
            _context.InsuranceBenefitType.Add(item);
            _context.SaveChanges();

            return item;

        }


        public InsuranceBenefitType Update(InsuranceBenefitType item)
        {

            String exception = string.Empty;

            var _InsuranceBenefitType = _context.InsuranceBenefitType.Find(item.Id);


            if (_InsuranceBenefitType == null)
                throw new AppException("Cover not found");

            // validation
            if (!ValidateRequireField(item, out exception))
                throw new AppException(exception);

            if (item.BenefitType.ToLower() != _InsuranceBenefitType.BenefitType.ToLower())
            {
                // Cover Name has changed so check if the new Cover Name is already exists
                if (_context.InsuranceBenefitType.Any(x => x.BenefitType == item.BenefitType))
                    throw new AppException("Benefit Type Name " + item.BenefitType + " is already exists");
            }

            _InsuranceBenefitType.BenefitType = item.BenefitType;
            _InsuranceBenefitType.ParentBenefitTypeID = item.ParentBenefitTypeID;
            _InsuranceBenefitType.RowOrder = item.RowOrder;
            _InsuranceBenefitType.DeletedAt = item.DeletedAt;
            _InsuranceBenefitType.UpdatedAt = DateTime.Now;

            _context.InsuranceBenefitType.Update(_InsuranceBenefitType);
            _context.SaveChanges();

            return item;

        }


        public void Delete(InsuranceBenefitType item)
        {
            var _InsuranceBenefitType = _context.InsuranceBenefitType.Find(item.Id);

            if (_InsuranceBenefitType != null)
            {
                _InsuranceBenefitType.DeletedAt = DateTime.Now;
                _InsuranceBenefitType.UpdatedAt = DateTime.Now;

                _context.InsuranceBenefitType.Update(_InsuranceBenefitType);
                _context.SaveChanges();
            }
            else
                throw new AppException("Cover not found");

        }


        private Boolean ValidateRequireField(InsuranceBenefitType item, out String exception)
        {

            exception = string.Empty;

            // validation
            if (string.IsNullOrWhiteSpace(item.BenefitType))
            {
                exception = "Cover Name is required";
                return false;
                //throw new AppException("LoginProviderId is required");
            }


            return true;

        }


    }

}
