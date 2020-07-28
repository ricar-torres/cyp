using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApi.Entities.Identity;
using WebApi.Helpers;
using WebApi.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    //Autorized default
    //Default route "api/[controller]" 
    public class MenuController : BaseController
    {

        private IMenuService _menuService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public MenuController(
            IMenuService menuService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _menuService = menuService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        // GET: api/menu/
        [HttpGet]
        public IActionResult Get()
        {

            var menu = _menuService.GetAllByUser(new AppUser { Id = GetNameClaim() });
            return Ok(menu);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
