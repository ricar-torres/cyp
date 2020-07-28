using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApi.Dtos;
using WebApi.Entities.Identity;
using WebApi.Helpers;

namespace WebApi.Services
{

    public interface IRoleService
    {
        IQueryable<AppRole> GetAll();
        AppRole GetById(int id);
        AppRole Create(AppRole role);
        void Update(AppRole role);
        void Delete(AppRole role);
    }

    public class RoleService : IRoleService
    {

        private DataContext _context;
        private readonly AppSettings _appSettings;

        public RoleService(DataContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        public IQueryable<AppRole> GetAll()
        {
            IQueryable<AppRole> payload = null;

            payload = _context.AppRole.AsQueryable();

            return payload.AsNoTracking();

        }

        public AppRole GetById(int id)
        {

            AppRole payload = null;

            payload = _context.AppRole.Find(id);

            return payload;
        }

        public AppRole Create(AppRole role)
        {

            //validation
            if (string.IsNullOrWhiteSpace(role.Name))
                throw new AppException("Name is required");

            if (_context.AppRole.Any(x => x.Name == role.Name))
                throw new AppException("Name " + role.Name + " is already taken");

            _context.AppRole.Add(role);

            _context.SaveChanges();

            return role;

        }

        public void Update(AppRole roleParam)
        {

            var role = _context.AppRole.Find(roleParam.Id);

            if (role == null)
                throw new AppException("Role not found");

            if (roleParam.Name.ToLower() != role.Name.ToLower())
            {
                // username has changed so check if the new username is already taken
                if (_context.AppRole.Any(x => x.Name == roleParam.Name))
                    throw new AppException("Name " + roleParam.Name + " is already taken");
            }

            // update user properties
            role.Name = roleParam.Name;
            role.Description = roleParam.Description;
            role.DelFlag = roleParam.DelFlag;
            role.FUpdUserId = roleParam.FUpdUserId;
            role.UpdDt = DateTime.Now;

            _context.AppRole.Update(role);
            _context.SaveChanges();

        }

        public void Delete(AppRole roleParam)
        {
            var role = _context.AppRole.Find(roleParam.Id);

            if (role != null)
            {

                role.UpdDt = DateTime.Now;
                role.DelFlag = true;

                _context.AppRole.Update(role);
                _context.SaveChanges();
            }
            else
                throw new AppException("Role not found");

        }


    }
}
