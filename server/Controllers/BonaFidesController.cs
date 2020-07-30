using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Services;

namespace WebApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class BonaFidesController : BaseController
  {
    private IBonaFidesServices _BonaFidesServices;
    private IMapper _mapper;
    private readonly AppSettings _appSettings;
    public BonaFidesController(
      IBonaFidesServices bonaFidesServices,
      IMapper mapper,
      IOptions<AppSettings> appSettings)
    {
      _BonaFidesServices = bonaFidesServices;
      _mapper = mapper;
      _appSettings = appSettings.Value;
    }


    [AllowAnonymous]
    [HttpGet()]
    public IActionResult GetAll()
    {
      try
      {
        var res = _BonaFidesServices.GetAll();
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

    [AllowAnonymous]
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
      try
      {
        var res = _BonaFidesServices.GetById(id);
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
    [AllowAnonymous]
    [HttpPost]
    public IActionResult Create([FromBody] BonaFides payload)
    {

      try
      {

        _BonaFidesServices.Create(payload);
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
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] BonaFides payload)
    {
      try
      {

        payload.Id = id;
        var res = _BonaFidesServices.Update(payload);

        return Ok(res);

      }
      catch (AppException ex)
      {
        // return error message if there was an exception
        return DefaultError(ex);
      }
    }

    //[Filters.Authorize(PermissionItem.User, PermissionAction.Delete)]
    [AllowAnonymous]
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
      try
      {

        _BonaFidesServices.Delete(id);
        return Ok();

      }
      catch (Exception ex)
      {
        // return error message if there was an exception
        return DefaultError(ex);
      }
    }

  }
}