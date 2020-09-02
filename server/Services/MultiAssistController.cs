using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers;
using WebApi.Entities;

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
			list.ForEach((x) => {
				var covers = new List<Covers>(x.Covers);
				covers.ForEach((c) => {
					c.HealthPlan = null;
				});
				x.Covers = covers;
			});
			return Ok(list);
		}
	}
}