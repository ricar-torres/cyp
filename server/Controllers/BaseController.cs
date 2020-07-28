using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class BaseController : Controller
    {

        // UserId Claim
        protected int GetNameClaim()
        {

            var claim = User.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault();

            if (claim != null)
                return int.Parse(claim.Value);
            else
                return 0;

        }

        // Email Claim
        protected string GetEmailClaim()
        {

            var claim = User.Claims.Where(c => c.Type == ClaimTypes.Email).FirstOrDefault();

            if (claim != null)
                return claim.Value;
            else
                return null;

        }

        // Application Claim
        protected int GetApplicationClaim()
        {

            var claim = User.Claims.Where(c => c.Type == ClaimTypes.GroupSid).FirstOrDefault();

            if (claim != null)
                return int.Parse(claim.Value);
            else
                return 0;

        }

        protected void AddPagingHeader(int totalPages)
        {

            Response.Headers.Add("X-Total-Count", totalPages.ToString());
            Response.Headers.Add("Access-Control-Expose-Headers", "X-Total-Count");

        }

        protected IActionResult DefaultError(Exception ex)
        {

            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                code = StatusCodes.Status500InternalServerError,
                error = ex.InnerException == null ? ex.Message : $"Message=>[  {ex.Message} ] InnerException=>[ {ex.InnerException.Message} ]"
            });

        }

    }
}