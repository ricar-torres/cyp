using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;
using WebApi.Entities;
using WebApi.Entities.Identity;
using WebApi.Helpers;
using WebApi.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{

    //Autorized default
    //Default route "api/[controller]" 
    public class RolesController : BaseController
    {

        private IRoleService _roleService;
        private IMapper _mapper;

        public RolesController(
            IRoleService roleService,
            IMapper mapper)
        {
            _roleService = roleService;
            _mapper = mapper;
        }

        // GET: api/roles
        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var roles = _roleService.GetAll();

                return Ok(roles);

            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(ex.Message);
            }

        }

        // GET api/roles/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var role = _roleService.GetById(id);
                return Ok(role);

            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(ex.Message);
            }
        }

        // POST api/roles
        [HttpPost]
        public IActionResult Create([FromBody]AppRole role)
        {

            try
            {
                // save 

                role.FCreateUserId = GetNameClaim();
                _roleService.Create(role);

                return Ok();

            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(ex.Message);
            }

        }

        // PUT api/roles/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody]AppRole role)
        {
            role.Id = id;

            try
            {

                // save 
                role.FUpdUserId = GetNameClaim();
                _roleService.Update(role);

                return Ok();

            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(ex.Message);
            }

        }

        // DELETE api/roles/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {

            try
            {

                AppRole AppRole = new AppRole
                {
                    Id = id,
                    FUpdUserId = GetNameClaim()
                };

                _roleService.Delete(AppRole);

                return Ok();

            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(ex.Message);
            }

        }
    }
}