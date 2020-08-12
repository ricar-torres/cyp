using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using server.Services;
using WebApi.Entities;
using WebApi.Helpers;

namespace WebApi.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class CommunicationMethodController : BaseController {

		readonly ICommunicationMethodService _service;
		public CommunicationMethodController(ICommunicationMethodService service) {
			this._service = service;
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
		[HttpGet("{id}", Name = "Get")]
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
		public IActionResult Create([FromBody] CommunicationMethods payload) {

			try {

				_service.Create(payload);
				return Ok(payload);

			} catch (AppException ex) {
				// return error message if there was an exception
				return DefaultError(ex);
			}
		}

		// PUT: api/CommunicationMethod/5
		[AllowAnonymous]
		[HttpPut("{id}")]
		public IActionResult Put(int id, [FromBody] CommunicationMethods payload) {
			try {

				payload.Id = id;
				var res = _service.Update(payload);

				return Ok(res);

			} catch (AppException ex) {
				// return error message if there was an exception
				return DefaultError(ex);
			}
		}

		// DELETE: api/ApiWithActions/5
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

		[AllowAnonymous]
		[HttpGet("[action]")]
		public IActionResult CheckNameExist([FromQuery] string name) {
			try {
				return Ok(_service.NameExists(name));
			} catch (Exception ex) {
				return DefaultError(ex);
			}
		}
	}
}