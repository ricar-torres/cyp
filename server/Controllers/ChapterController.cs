using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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
  public class ChapterController : BaseController
  {
    private IChapterServices _service;
    private IMapper _mapper;
    private readonly AppSettings _appSettings;

    public ChapterController(
        IChapterServices service,
        IMapper mapper,
        IOptions<AppSettings> appSettings)
    {
      _service = service;
      _mapper = mapper;
      _appSettings = appSettings.Value;
    }

    [Authorize]
    [HttpPost("client")]
    public async Task<IActionResult> SaveChaoterClient(ChapterClient payload)
    {
      try
      {
        await _service.saveClientChapter(payload);
        return Ok();
      }
      catch (Exception ex)
      {
        return DefaultError(ex);
      }
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

    [Authorize]
    [HttpGet("bonafide/{id}")]
    public async Task<IActionResult> GetByBonafideId(int id)
    {
      try
      {
        var res = await _service.GetByBonafineId(id);
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
    [HttpGet("{clientId}/{bonafideId}")]
    public async Task<IActionResult> GetChapterOfClientByBonafideId(int clientId, int bonafideId)
    {
      try
      {
        var res = await _service.GetChapterOfClientByBonafideId(clientId, bonafideId);
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
    [HttpDelete("{clientId}/{bonafideId}")]
    public async Task<IActionResult> DeleteClientChapter(int clientId, int bonafideId)
    {
      try
      {
        await _service.DeleteClientChapter(clientId, bonafideId);
        return Ok();
      }
      catch (Exception ex)
      {
        return DefaultError(ex);
      }
    }

    [Authorize]
    [HttpGet("bonafides")]
    public async Task<IActionResult> GetChaptersInBonafides([FromQuery] String bonafideIds)
    {
      try
      {
        var res = await _service.getChaptersInBonafideIds(bonafideIds);
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
    public IActionResult Create([FromBody] Chapters payload)
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
    public IActionResult Update(int id, [FromBody] Chapters payload)
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
    [HttpGet("CheckName/{bonafideid}/{name}")]
    public async Task<IActionResult> checkName(string name, int bonafideid)
    {
      try
      {
        var check = await _service.ChekcName(name, bonafideid);
        return Ok(check);
      }
      catch (Exception ex)
      {
        return DefaultError(ex);
      }
    }
  }

}