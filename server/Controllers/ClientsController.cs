using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using server.Dtos;
using System;
using System.Threading.Tasks;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Services;

namespace WebApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ClientsController : BaseController
  {
    private IClientService _service;
    private IMapper _mapper;
    private readonly AppSettings _appSettings;

    public ClientsController(
        IClientService service,
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
    public IActionResult Create([FromBody] ClientInformationDto payload)
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
    [HttpPut()]
    public async Task<IActionResult> Update(ClientInformationDto payload)
    {
      try
      {
        var res = await _service.Update(payload);
        return Ok();
      }
      catch (AppException ex)
      {
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