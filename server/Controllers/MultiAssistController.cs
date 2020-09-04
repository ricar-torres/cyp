using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Services;
using WebApi.Controllers;
using WebApi.Entities;

namespace WebApi.Controllers {
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
					var masList = new List<MultiAssists>(c.MultiAssists);
					masList.ForEach((ma) => {
						ma.Cover = null;
					});
					c.MultiAssists = masList;
				});
				x.Covers = covers;
			});
			return Ok(list);
		}

		[AllowAnonymous]
		[HttpPost]
		public IActionResult AttatchMultiAssist([FromBody] Covers payload) {
			return Ok(payload);
		}
	}
}