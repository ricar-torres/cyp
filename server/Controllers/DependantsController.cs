using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Dtos;
using server.Entities;
using server.Services;
using WebApi.Controllers;
using WebApi.Entities;
using WebApi.Helpers;

namespace server.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class DependantsController : BaseController
  {
    readonly IDependantService _service;
    readonly DataContext _context;
    readonly IMapper _mapper;
    public DependantsController(IDependantService service, DataContext context, IMapper mapper)
    {
      this._service = service;
      this._context = context;
      this._mapper = mapper;
    }
    // GET: api/CommunicationMethod
    [AllowAnonymous]
    [HttpGet]
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

    // GET: api/CommunicationMethod/5
    [AllowAnonymous]
    [HttpGet("{id}")]
    public IActionResult Get(int id)
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

    // POST: api/CommunicationMethod
    [AllowAnonymous]
    [HttpPost]
    public IActionResult Create([FromBody] DependentDto payload)
    {
      var x = payload;
      return Ok(payload);
      // try {
      // 	_service.Create(payload);
      // 	return Ok(payload);

      // } catch (AppException ex) {
      // 	// return error message if there was an exception
      // 	return DefaultError(ex);
      // }
    }

    [AllowAnonymous]
    [HttpGet("[action]/{clientId:int}")]
    public IActionResult GetAllByClient(int clientId)
    {
      List<DependentDto> dependentsDtoList = new List<DependentDto>();
      List<Dependents> dependents;
      IEnumerable<TypeOfRelationship> relationships;
      try
      {
        dependents = new List<Dependents>(_service.GetAllByClient(clientId).Where(x => x.Relationship.HasValue).Take(1));
        if (dependents is object)
        {
          relationships = _service.GetRelationTypes();
          dependents.ForEach((item) =>
          {
            dependentsDtoList.Add(_mapper.Map<DependentDto>(item));
            var itemDto = dependentsDtoList.Last();
            itemDto.RelationshipType = relationships.Where(_ => _.Id == item.Relationship).FirstOrDefault();
            itemDto.CoverName = item.Cover is Covers c ? c.Name : string.Empty;
            itemDto.Cover = null;
          });
          return Ok(dependentsDtoList);
        }
        else
        {
          return NotFound();
        }
      }
      catch (Exception ex)
      {
        return DefaultError(ex);
      }
    }

    [AllowAnonymous]
    [HttpGet("[action]")]
    public IActionResult GetRelationTypes()
    {
      try
      {
        var res = _service.GetRelationTypes();
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

    [HttpGet("test")]
    public IActionResult GetCovers()
    {
      try
      {
        var res = this._service.GetCovers();
        return Ok(res);
      }
      catch (System.Exception)
      {

        throw;
      }
    }

    [HttpGet("test1")]
    public IActionResult GetHealthPlans()
    {
      try
      {
        var res = this._service.GetHealthPlans();
        return Ok(res);
      }
      catch (System.Exception)
      {

        throw;
      }
    }
  }
}