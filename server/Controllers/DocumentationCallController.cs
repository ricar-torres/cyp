using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Services;
using WebApi.Controllers;
using WebApi.Entities;
using WebApi.Helpers;

namespace server.Controllers {

	[Route("api/[controller]")]
	[ApiController]
	public class DocumentationCallController : BaseController {
		readonly IDocumentationCallService _service;
		public DocumentationCallController(IDocumentationCallService service) {
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
		public async Task<IActionResult> Create([FromBody] ClientUser payload) {

			try {

				payload.ConfirmationNumber = await this._service.CreateConfirmationCode();
				_service.Create(payload);
				return Ok(payload);

			} catch (AppException ex) {
				// return error message if there was an exception
				return DefaultError(ex);
			}
		}

		[AllowAnonymous]
		[HttpGet]
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
				var res = _service.GetCallReasons();
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