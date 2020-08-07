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
  public class AddressController : BaseController
  {
    private IAddressService _service;
    private IMapper _mapper;
    private readonly AppSettings _appSettings;

    public AddressController(
        IAddressService service,
        IMapper mapper,
        IOptions<AppSettings> appSettings)
    {
      _service = service;
      _mapper = mapper;
      _appSettings = appSettings.Value;
    }


    [Authorize]
    [HttpGet("{clientId}")]
    public async Task<IActionResult> GetClientAddresses(int clientId)
    {
      try
      {
        var check = await _service.GetClientAddress(clientId);
        return Ok(check);
      }
      catch (Exception ex)
      {
        return DefaultError(ex);
      }
    }

    [Authorize]
    [HttpPut("{clientId}")]
    public async Task<IActionResult> UpdateClientService([FromBody] List<Addresses> addresses, int clientId)
    {
      try
      {
        var check = await _service.UpdateClientAddress(addresses, clientId);
        return Ok(check);
      }
      catch (Exception ex)
      {
        return DefaultError(ex);
      }
    }
  }

}