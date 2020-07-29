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
    public class AuthController : BaseController
    {

        private IAuthService _authService;
        private IApplicationService _appService;
        private IOtpService _otpService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public AuthController(
            IAuthService authService,
            IApplicationService appService,
            IMapper mapper,
            IOtpService otpService,
        IOptions<AppSettings> appSettings)
        {
            _authService = authService;
            _appService = appService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _otpService = otpService;
        }

        [AllowAnonymous]
        [HttpPost("token")]
        public IActionResult Signin([FromBody] UserDto payload)
        {
            try
            {
                var res = _authService.Login(payload.ApplicationId, payload.UserName, payload.Password, payload.UserType);
                return res;
            }
            catch (Exception ex)
            {
                //_logger.LogError("FAILED: GetMerchant", result.Error);
                return DefaultError(ex);

            }
        }

        [AllowAnonymous]
        [HttpPost("signup")]
        public IActionResult Signup([FromBody] UserDto payload)
        {
            try
            {

                var user = _authService.SignUp(payload);

                if (user != null) {
                    var res = _authService.Login(payload.ApplicationId, payload.UserName, payload.Password, payload.UserType);
                    return res;
                }

                return BadRequest();

            }
            catch (Exception ex)
            {
                //throw new ArgumentException(string.IsNullOrEmpty(ex.InnerException.Message) ? ex.Message : ex.InnerException.Message);
                return DefaultError(ex);
            }
        }

        [AllowAnonymous]
        [HttpGet("checkUserName/{applicationId}/{username}")]
        public IActionResult CheckEmail(int applicationId, string username)
        {
            try
            {
                var user = _authService.CheckUserName(applicationId, username);
                if (user != null)
                {
                    return Ok(false);
                }

                return Ok(true);

            }
            catch (Exception ex)
            {
                return DefaultError(ex);
            }
        }

       

        [AllowAnonymous]
        [HttpPost("validateOtp/{otp}")]
        public IActionResult validateOtp([FromBody] AppUser user, string otp)
        {
            try
            {
                var res = _otpService.Validate(OtpType.EMAIL_VALIDATION, user, otp);
                if (res == true)
                    return Ok(new {
                        valid = true
                    });
                else
                    return Forbid();
            }
            catch (Exception ex)
            {
                return DefaultError(ex);
            }
        }

    }
}