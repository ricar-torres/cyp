using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using server.Dtos;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Services;

namespace WebApi.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class CampaignsController : BaseController {
		private ICampaignsService _service;
		private IMapper _mapper;
		private readonly AppSettings _appSettings;

		public CampaignsController(
			ICampaignsService service,
			IMapper mapper,
			IOptions<AppSettings> appSettings) {
			_service = service;
			_mapper = mapper;
			_appSettings = appSettings.Value;
		}

		[AllowAnonymous]
		[HttpGet]
		public IActionResult GetAll() {
			try {
				var res = _service.GetAll();
				// var campaignList = _mapper.Map<List<CampaignDto>>(res);
				if (res == null) {
					return NotFound();
				}

				return Ok(res);

			} catch (Exception ex) {
				return DefaultError(ex);
			}
		}

		[AllowAnonymous]
		[HttpGet("{id}")]
		public IActionResult GetById(int id) {
			try {
				var res = _service.GetById(id);
				if (res == null) {
					return NotFound();
				}

				return Ok(res);

			} catch (Exception ex) {
				return DefaultError(ex);
			}
		}

		[AllowAnonymous]
		[HttpGet("[action]")]
		public IActionResult CheckCampaignNameExist([FromQuery] string name) {
			try {
				return Ok(_service.NameExists(name));
			} catch (Exception ex) {
				return DefaultError(ex);
			}
		}

		//[Filters.Authorize(PermissionItem.User, PermissionAction.Create)]
		[AllowAnonymous]
		[HttpPost]
		public IActionResult Create([FromBody] Campaigns payload) {

			try {

				_service.Create(payload);
				return Ok(payload);

			} catch (AppException ex) {
				// return error message if there was an exception
				return DefaultError(ex);
			}
		}

		//[Filters.Authorize(PermissionItem.User, PermissionAction.Update)]
		[AllowAnonymous]
		[HttpPut("{id}")]
		public IActionResult Update(int id, [FromBody] Campaigns payload) {
			try {

				payload.Id = id;
				var res = _service.Update(payload);

				return Ok(res);

			} catch (AppException ex) {
				// return error message if there was an exception
				return DefaultError(ex);
			}
		}

		//[Filters.Authorize(PermissionItem.User, PermissionAction.Delete)]
		[AllowAnonymous]
		[HttpDelete("{id}")]
		public IActionResult Delete(int id) {
			try {

				_service.Delete(id);
				return Ok();

			} catch (Exception ex) {
				// return error message if there was an exception
				return DefaultError(ex);
			}
		}
	}

}