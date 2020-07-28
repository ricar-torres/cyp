using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using WebApi.Dtos;
using WebApi.Entities.Identity;
using WebApi.Enums;
using WebApi.Helpers;
using WebApi.Services;

namespace WebApi.Controllers
{

    //Autorized default
    //Default route "api/[controller]" 
    public class UsersController : BaseController
    {
        private IUserService _userService;
        private IMapper _mapper;
        
        public UsersController(
            IUserService userService,
            IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [Filters.Authorize(PermissionItem.User, PermissionAction.Create)]
        [HttpPost]
        public IActionResult Create([FromBody] UserDto paramUserDto)
        {
            // map dto to entity
            var user = _mapper.Map<AppUser>(paramUserDto);

            try
            {
                // save 

                user.FCreateUserId = GetNameClaim();
                user.UserType = UserType.ADMIN;
                user.ApplicationId = GetApplicationClaim();

                _userService.Create(user, paramUserDto.Password);

                var userDto = _mapper.Map<UserDto>(user);

                return Ok(userDto);

            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return DefaultError(ex);
            }
        }

        [Filters.Authorize(PermissionItem.User, PermissionAction.Read)]
        [HttpGet]
        public IActionResult Get()
        {
            try
            {

                var users = _userService.GetAll(GetApplicationClaim());
                var UsersDto = _mapper.Map<List<UserDto>>(users);
                //var data = from item in checkDto select new { item.PKReturnedCheck, item.checkNo, item.name, item.amount, item.checkDate, item.statusId, item.Status.StatusDesc, item.ssnMasked, item.dateDelivered, item.comment };


                return Ok(UsersDto);

            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return DefaultError(ex);
            }
        }

        [Filters.Authorize(PermissionItem.User, PermissionAction.Read)]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var user = _userService.GetById(GetApplicationClaim(), id);
                var userDto = _mapper.Map<UserDto>(user);
                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return DefaultError(ex);
            }
        }

        [Filters.Authorize(PermissionItem.User, PermissionAction.Update)]
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UserDto paramUserDto)
        {
            // map dto to entity and set id
            var user = _mapper.Map<AppUser>(paramUserDto);
            user.Id = id;

            try
            {

                // save 
                user.FUpdUserId = GetNameClaim();
                user = _userService.Update(user, paramUserDto.Password);

                var userDto = _mapper.Map<UserDto>(user);

                return Ok(userDto);

            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return DefaultError(ex);
            }
        }

        [Filters.Authorize(PermissionItem.User, PermissionAction.Delete)]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {

                AppUser user = new AppUser
                {
                    Id = id,
                    FUpdUserId = GetNameClaim()
                };

                _userService.Delete(user);

                return Ok();

            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return DefaultError(ex);
            }
        }

        [Filters.Authorize(PermissionItem.User, PermissionAction.Create)]
        [HttpPost("{userId}/roles/{roleId}")]
        public IActionResult AddRole(int userId, int roleId)
        {
            try
            {

                UserRole userRole = new UserRole
                {
                    UserId = userId,
                    RoleId = roleId
                };

                _userService.AddRole(userRole);

                return Ok(userRole);

            }
            catch (AppException ex)
            {
                return DefaultError(ex);
            }
        }

        [Filters.Authorize(PermissionItem.User, PermissionAction.Delete)]
        [HttpDelete("{userId}/roles/{roleId}")]
        public IActionResult DeleteRole(int userId, int roleId)
        {
            try
            {
                _userService.DeleteRole(userId, roleId);
                return Ok();
            }
            catch (Exception ex)
            {
                return DefaultError(ex);
            }
        }

        [Authorize]
        [HttpPatch("ChangePassword")]
        public IActionResult ChangePassword([FromBody] UserDto paload)
        {
            try
            {
                paload.Id = GetNameClaim();
                var res = _userService.ChangePasswod(paload);

                return StatusCode(res);

            }
            catch (Exception ex)
            {
                return DefaultError(ex);
            }
        }
    }
}