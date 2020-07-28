using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using WebApi.Dtos;
using WebApi.Entities.Identity;
using WebApi.Enums;
using WebApi.Helpers;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : BaseController
    {
        private IApplicationService _appService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public ApplicationController(
            IApplicationService appService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _appService = appService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpGet("{key}")]
        public IActionResult GetByKey(string key)
        {
            try
            {
                var res = _appService.GetByKey(key);
                if (res == null)
                {
                    return NotFound();
                }

                return Ok(new {
                    res.Id,
                    res.Key,
                    res.Name,
                    res.PrimaryColor
                });

            }
            catch (Exception ex)
            {
                return DefaultError(ex);
            }
        }
    }
    
}