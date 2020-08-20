using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Services;

namespace WebApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CoversController : BaseController
  {
    private ICoverService _service;
    private IMapper _mapper;
    private readonly AppSettings _appSettings;

    public CoversController(
        ICoverService service,
        IMapper mapper,
        IOptions<AppSettings> appSettings)
    {
      _service = service;
      _mapper = mapper;
      _appSettings = appSettings.Value;
    }

    [Authorize]
    [HttpGet()]
    public async Task<IActionResult> GetAll()
    {
      try
      {
        var res = await _service.GetAll();
        if (res == null)
        {
          return NotFound();
        }
        return Ok(res);
      }
      catch (Exception ex)
      {
        return DefaultError(ex);
      }
    }

    [Authorize]
    [HttpGet("plan/{coverId}")]
    public async Task<IActionResult> GetPlanByCover(int coverId)
    {
      try
      {
        var res = await _service.GetPlanByCover(coverId);
        return Ok(res);
      }
      catch (Exception ex)
      {
        return DefaultError(ex);
      }
    }

    [Authorize]
    [HttpGet("{planId}")]
    public async Task<IActionResult> GetByPlan(int planId)
    {
      try
      {
        var res = await _service.GetByPlan(planId);
        if (res == null)
        {
          return NotFound();
        }
        return Ok(res);
      }
      catch (Exception ex)
      {
        return DefaultError(ex);
      }
    }
  }
}