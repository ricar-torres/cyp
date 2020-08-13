using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Services;

namespace WebApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class QualifyingEventsController : BaseController
  {

    private IQualifyingEventsSerivie _service;
    private IMapper _mapper;
    private readonly AppSettings _appSettings;

    public QualifyingEventsController(
        IQualifyingEventsSerivie service,
        IMapper mapper,
        IOptions<AppSettings> appSettings)
    {
      _service = service;
      _mapper = mapper;
      _appSettings = appSettings.Value;
    }

    [Authorize]
    [HttpGet()]
    public IActionResult GetAll()
    {
      try
      {
        var res = _service.GetAll();
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
    public IActionResult Create([FromBody] QualifyingEvents payload)
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
    public IActionResult Update(int id, [FromBody] QualifyingEvents payload)
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

    [Authorize]
    [HttpGet("CheckName/{name}")]
    public async Task<IActionResult> checkName(string name)
    {
      try
      {
        var check = await _service.ChekcName(name);
        return Ok(check);
      }
      catch (Exception ex)
      {
        return DefaultError(ex);
      }
    }
  }

}