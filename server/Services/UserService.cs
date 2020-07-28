using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WebApi.Dtos;
using WebApi.Entities.Identity;
using WebApi.Enums;
using WebApi.Helpers;

namespace WebApi.Services
{
    public interface IUserService
    {
        IQueryable<AppUser> GetAll(int ApplicationID);
        AppUser GetById(int ApplicationId, int id);
        AppUser Create(AppUser user, string password);
        AppUser Update(AppUser user, string password = null);
        void Delete(AppUser userParam);
        UserRole AddRole(UserRole userRole);
        int ChangePasswod(UserDto payload);
        void DeleteRole(int userId, int roleId);
    }

    public class UserService : IUserService
    {
        private DataContext _context;
        private IAuthService _authService;
        private IMapper _mapper;
 
        public UserService(DataContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public IQueryable<AppUser> GetAll(int ApplicationId)
        {
            IQueryable<AppUser> payload = null;

            try
            {

                payload = _context.AppUser.Include(user => user.LoginProvider).Where(x => x.ApplicationId == ApplicationId && x.UserType == UserType.ADMIN).AsQueryable();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return payload.AsNoTracking();

        }

        public AppUser GetById(int ApplicationId, int id)
        {

            AppUser payload = null;

            try
            {

                payload = _context.AppUser.Include(u => u.Roles).ThenInclude(r => r.Role).Where(u => u.Id == id && u.ApplicationId == ApplicationId).FirstOrDefault();

                if (payload != null)
                {

                    payload.RolesAlt = payload.Roles.Select(r => r.RoleId).ToArray();

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }


            return payload;
        }

        public AppUser Create(AppUser user, string password)
        {

            String exception = string.Empty;

            try
            {

                // validation
                //if (user.LoginProviderId == 0)
                //    throw new AppException("LoginProviderId is required");

                //if (!_context.LoginProvider.Any(lp => lp.Id == user.LoginProviderId && lp.DelFlag == false))
                //    throw new AppException("LoginProviderId is invalid");

                //if (string.IsNullOrWhiteSpace(password) && user.LoginProviderId == (int)LoginProviderEnum.Local)
                //    throw new AppException("Password is required");

                //if (string.IsNullOrWhiteSpace(user.UserName))
                //    throw new AppException("UserName is required");

                if (!_authService.ValidateUserRequireField(user, password, out exception))
                    throw new AppException(exception);

                if (_context.AppUser.Any(x => x.UserName == user.UserName))
                    throw new AppException("UserName " + user.UserName + " is already taken");


                byte[] passwordHash, passwordSalt;
                _authService.CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.IsChgPwd =  true;

                _context.AppUser.Add(user);
                _context.SaveChanges();

                var userDto = _mapper.Map<UserDto>(user);

                syncRoles(userDto);

                return user;

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public AppUser Update(AppUser userParam, string password = null)
        {

            try
            {

                String exception = string.Empty;
                var user = _context.AppUser.Find(userParam.Id);

                if (user == null)
                    throw new AppException("User not found");

                //if (!ValidateUserRequireField(userParam, password, out exception))
                //    throw new AppException(exception);

                if (userParam.UserName.ToLower() != user.UserName.ToLower())
                {
                    // username has changed so check if the new username is already taken
                    if (_context.AppUser.Any(x => x.UserName == userParam.UserName))
                        throw new AppException("UserName " + userParam.UserName + " is already taken");
                }

                // update user properties
                user.FirstName = userParam.FirstName;
                user.LastName = userParam.LastName;
                user.UserName = userParam.UserName;
                user.Email = userParam.Email;
                user.LoginProviderId = userParam.LoginProviderId;
                user.DelFlag = userParam.DelFlag;
                user.FUpdUserId = userParam.FUpdUserId;
                user.UpdDt = DateTime.Now;



                // update password if it was entered
                if (!string.IsNullOrWhiteSpace(password))
                {
                    byte[] passwordHash, passwordSalt;
                    _authService.CreatePasswordHash(password, out passwordHash, out passwordSalt);

                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                }

                _context.AppUser.Update(user);
                _context.SaveChanges();

                var userDto = _mapper.Map<UserDto>(userParam);

                syncRoles(userDto);

                return user;

            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        public void Delete(AppUser userParam)
        {
            var user = _context.AppUser.Find(userParam.Id);

            if (user != null)
            {

                user.UpdDt = DateTime.Now;
                user.DelFlag = true;

                _context.AppUser.Update(user);
                _context.SaveChanges();
            }
            else
                throw new AppException("User not found");

        }

        public UserRole AddRole(UserRole userRole)
        {

            var user = _context.AppUser.Find(userRole.UserId);

            if (!_context.AppUser.Any(u => u.Id == userRole.UserId))
                throw new AppException("User not found");

            if (!_context.AppRole.Any(r => r.Id == userRole.RoleId && r.DelFlag == false))
                throw new AppException("Role not found");

            if (_context.UserRole.Any(r => r.UserId == userRole.UserId && r.RoleId == userRole.RoleId))
                throw new AppException("RoleId " + userRole.RoleId + " is already exists");

            _context.UserRole.Add(userRole);
            _context.SaveChanges();

            return userRole;

        }

        public void DeleteRole(int userId, int roleId)
        {

            var userRole = _context.UserRole.Find(userId, roleId);

            if (userRole != null)
            {
                _context.UserRole.Remove(userRole);
                _context.SaveChanges();
            }

        }

        public void syncRoles(UserDto user)
        {

            if (user.RolesAlt != null)
            {

                user.Roles.Clear();

                foreach (var role in user.RolesAlt)
                {
                    //if (role.UserId != userId && role.UserId > 0)
                    //    throw new AppException("Roles.UserId " + role.UserId + " is invalid");
                    //else
                    //role.UserId = userId;

                    user.Roles.Add(new UserRole { UserId = user.Id, RoleId = role });

                }

                var roleList = _context.UserRole.Where(r => r.UserId == user.Id).ToList();

                var addList = user.Roles.Where(r => !roleList.Any(rl => rl.RoleId == r.RoleId) && _context.AppRole.Any(ar => ar.Id == r.RoleId)).GroupBy(p => (p.UserId, p.RoleId)).Select(g => g.First()).ToList();
                //var addList = user.RolesAlt.Where(r => !roleList.Any(rl => rl.RoleId == r) && _context.AppRole.Any(ar => ar.Id == r)).GroupBy(p => (p.UserId, p.RoleId)).Select(g => g.First()).ToList();
                var removeList = roleList.Where(rl => !user.Roles.Any(r => r.RoleId == rl.RoleId)).GroupBy(p => (p.UserId, p.RoleId)).Select(g => g.First()).ToList();

                if (addList.Count() > 0)
                    _context.UserRole.AddRange(addList);

                if (removeList.Count() > 0)
                    _context.UserRole.RemoveRange(removeList);

                if (addList.Count() > 0 || removeList.Count() > 0)
                    _context.SaveChanges();

            }
        }

        public int ChangePasswod(UserDto payload)
        {
            var user = _context.AppUser.Find(payload.Id);
            if (user.IsChgPwd) payload.Password = payload.NewPassword;
            if (user == null) return StatusCodes.Status404NotFound;
            if (payload.Password == null || payload.NewPassword == null) return StatusCodes.Status400BadRequest;

            if (!user.IsChgPwd)
            {
                if (user.LoginProviderId == (int)LoginProviderEnum.Local)
                {
                    // check if password is correct
                    if (!_authService.VerifyPasswordHash(payload.Password, user.PasswordHash, user.PasswordSalt))
                        return StatusCodes.Status403Forbidden;
                }
                else
                {
                    if (user.LoginProviderId == (int)LoginProviderEnum.LDAP)
                    {
                        return StatusCodes.Status403Forbidden;
                    }
                }
            }

            byte[] passwordHash, passwordSalt;
            _authService.CreatePasswordHash(payload.NewPassword, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.IsChgPwd = false;

            _context.AppUser.Attach(user);
            _context.Entry(user).Property(x => x.PasswordHash).IsModified = true;
            _context.Entry(user).Property(x => x.PasswordSalt).IsModified = true;
            _context.Entry(user).Property(x => x.IsChgPwd).IsModified = true;

            _context.SaveChanges();

            return StatusCodes.Status200OK;
        }

    }
}