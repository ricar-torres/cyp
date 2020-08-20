using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Dtos;
using server.Services;
using WebApi.Controllers;
using WebApi.Entities;
using WebApi.Helpers;

namespace server.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class DependantsController : BaseController {
		readonly IDependantService _service;
		readonly DataContext _context;
		readonly IMapper _mapper;
		public DependantsController(IDependantService service, DataContext context, IMapper mapper) {
			this._service = service;
			this._context = context;
			this._mapper = mapper;
		}
		// GET: api/CommunicationMethod
		[AllowAnonymous]
		[HttpGet]
		public IActionResult GetAll() {
			try {
				var res = _service.GetAll();
				if (res == null) {
					return NotFound();
				}

				return Ok(res);

			} catch (Exception ex) {
				return DefaultError(ex);
			}
		}

		// GET: api/CommunicationMethod/5
		[AllowAnonymous]
		[HttpGet("{id}")]
		public IActionResult Get(int id) {
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

		// POST: api/CommunicationMethod
		[AllowAnonymous]
		[HttpPost]
		public IActionResult Create([FromBody] ClientUser payload) {

			// try {
			// 	payload.ConfirmationNumber = payload.ConfirmationNumber.Contains("000000000000") ?
			// 		CreateConfirmationCode() : payload.ConfirmationNumber;
			// 	_service.Create(payload);
			// 	return Ok(payload);

			// } catch (AppException ex) {
			// 	// return error message if there was an exception
			// 	return DefaultError(ex);
			// }
			return Ok();
		}

		[AllowAnonymous]
		[HttpGet("[action]/{clientId:int}")]
		public IActionResult GetAllByClient(int clientId) {
			try {
				var res = _service.GetAllByClient(clientId);
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
		public IActionResult GetCallTypes() {
			try {
				var res = _service.GetRelationTypes();
				if (res == null) {
					return NotFound();
				}
				return Ok(res);

			} catch (Exception ex) {
				return DefaultError(ex);
			}
		}

	}
}