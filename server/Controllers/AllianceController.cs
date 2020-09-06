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
    public async Task<IActionResult> Create([FromBody] AllianceDto payload)
    {

      try
      {
        if (payload.Id == null)
          await _service.Create(payload);
        else
          await this.Update(payload);
        return Ok(payload);

      }
      catch (AppException ex)
      {
        // return error message if there was an exception
        return DefaultError(ex);
      }
    }

    //[Filters.Authorize(PermissionItem.User, PermissionAction.Update)]

    [AllowAnonymous]
    [HttpPost("Cost/{id}")]
    public async Task<IActionResult> Cost(int id)
    {
      try
      {

        var res = await _service.UpdateCost(id);

                return Ok();
                //return Ok(res);
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return DefaultError(ex);
            }
        }

        [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromBody] AllianceDto payload)
    {
      try
      {
        await _service.Update(payload);
        return Ok();

      }
      catch (AppException ex)
      {
        // return error message if there was an exception
        return DefaultError(ex);
      }
    }


        //[Authorize]
        //[HttpPut("{id}")]
        //public IActionResult Update(int id, [FromBody] Alianzas payload)
        //{
        //    try
        //    {

        //        payload.Id = id;
        //        var res = _service.Update(payload);

        //        return Ok(res);

        //    }
        //    catch (AppException ex)
        //    {
        //        // return error message if there was an exception
        //        return DefaultError(ex);
        //    }
        //}

        //[Filters.Authorize(PermissionItem.User, PermissionAction.Delete)]
        [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      try
      {

        await _service.Delete(id);
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
    [HttpGet("AlianceRequest/{clientId}")]
    public async Task<IActionResult> AlianceRequest(int clientId)
    {
      try
      {
        var availableHP = await _service.AvailableHealthPlansForClient(clientId);
        return Ok(availableHP);
      }
      catch (Exception ex)
      {
        // return error message if there was an exception
        return DefaultError(ex);
      }
    }

    //[Filters.Authorize(PermissionItem.User, PermissionAction.Delete)]
    [Authorize]
    [HttpGet("CheckSsn/{ssn}")]
    public async Task<IActionResult> CheckSsn(string ssn)
    {
      try
      {
        var ssnAvailability = await _service.CheckSsn(ssn);
        return Ok(ssnAvailability);
      }
      catch (Exception ex)
      {
        // return error message if there was an exception
        return DefaultError(ex);
      }
    }

    //[Filters.Authorize(PermissionItem.User, PermissionAction.Delete)]
    [Authorize]
    [HttpGet("client/{clientid}/iselegible")]
    public async Task<IActionResult> ValidateClient(int clientid)
    {
      try
      {
        var reasons = await _service.IsElegible(clientid);
        return Ok(reasons);
      }
      catch (Exception ex)
      {
        // return error message if there was an exception
        return DefaultError(ex);
      }
    }

    //[Filters.Authorize(PermissionItem.User, PermissionAction.Delete)]
    [Authorize]
    [HttpGet("AffiliationTypes")]
    public async Task<IActionResult> GetAllAffTypes()
    {
      try
      {
        var reasons = await _service.GetAllAffTypes();
        return Ok(reasons);
      }
      catch (Exception ex)
      {
        // return error message if there was an exception
        return DefaultError(ex);
      }
    }


  }
}
