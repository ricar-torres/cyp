using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using server.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Services;

namespace WebApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AllianceController : BaseController
  {

    private IAllianceService _service;
    private IMapper _mapper;
    private readonly AppSettings _appSettings;

    public AllianceController(
    IAllianceService service,
    IMapper mapper,
    IOptions<AppSettings> appSettings)
    {
      _service = service;
      _mapper = mapper;
      _appSettings = appSettings.Value;
    }


    [AllowAnonymous]
    [HttpGet("getall/{clientId}")]
    public async Task<IActionResult> GetAll(int? clientId)
    {
      try
      {
        var res = new List<AllianceDto>();
        if (clientId == null)
        {
          res = await _service.GetAll(null);
        }
        else
        {
          res = await _service.GetAll(clientId);
        }

        return Ok(res);

      }
      catch (Exception ex)
      {
        return DefaultError(ex);
      }
    }

    [Authorize]
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
      try
      {
        var res = _service.GetById(id);
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

    //[Filters.Authorize(PermissionItem.User, PermissionAction.Create)]
    [Authorize]
    [HttpPost]
    public IActionResult Create([FromBody] Alianzas payload)
    {

      try
      {

        _service.Create(payload);
        return Ok(payload);

      }
      catch (AppException ex)
      {
        // return error message if there was an exception
        return DefaultError(ex);
      }
    }

    //[Filters.Authorize(PermissionItem.User, PermissionAction.Update)]
    [Authorize]
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Alianzas payload)
    {
      try
      {

        payload.Id = id;
        var res = _service.Update(payload);

        return Ok(res);

      }
      catch (AppException ex)
      {
        // return error message if there was an exception
        return DefaultError(ex);
      }
    }

    //[Filters.Authorize(PermissionItem.User, PermissionAction.Delete)]
    [Authorize]
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
      try
      {

        _service.Delete(id);
        return Ok();

      }
      catch (Exception ex)
      {
        // return error message if there was an exception
        return DefaultError(ex);
      }
    }

    //[Filters.Authorize(PermissionItem.User, PermissionAction.Delete)]
    [Authorize]
    [HttpGet("AlianceRequest")]
    public async Task<IActionResult> AlianceRequest([FromQuery] AlianceRequestDto payload)
    {
      try
      {
        var availableHP = await _service.AvailableHealthPlansForClient(payload);
        return Ok(availableHP);
      }
      catch (Exception ex)
      {
        // return error message if there was an exception
        return DefaultError(ex);
      }
    }
  }
}
