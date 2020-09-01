using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers;

namespace server.Services {
	public class MultiAssistController : BaseController {
		readonly IMultiAssistService _service;
		public MultiAssistController(IMultiAssistService service) {
			this._service = service;
		}

		[AllowAnonymous]
		[HttpGet]
		public IActionResult GetAll() {
			var list = _service.GetAllMultiAssist().ToList();
			return Ok(list);
		}
	}
}