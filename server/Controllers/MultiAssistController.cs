using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Dtos;
using server.Services;
using WebApi.Controllers;
using WebApi.Entities;

namespace WebApi.Controllers {
	public class MultiAssistController : BaseController {
		readonly IMultiAssistService _service;
		readonly IClientProductService _clientProductService;
		readonly IBeneficiaryService _beneficiaryService;
		public MultiAssistController(IMultiAssistService service, IClientProductService cpservice, IBeneficiaryService beneficiaryService) {
			this._service = service;
			this._clientProductService = cpservice;
			this._beneficiaryService = beneficiaryService;
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
		public IActionResult Create([FromBody] MultiAssistDto payload) {

			try {
				ClientProduct clientProduct = new ClientProduct {
					ClientId = payload.ClientId,
						ProductId = 2, //MULTI-ASSIST ID
						Status = 0,
						CreatedAt = DateTime.Now
				};
				int cpId;
				int masId;

				cpId = _clientProductService.Create(clientProduct);

				if (cpId > 0) {
					payload.Payload.ClientProductId = cpId;
					masId = _service.Create(payload.Payload);
				} else {
					return BadRequest();
				}

				return Ok(payload);

			} catch (System.Exception ex) {
				return DefaultError(ex);
			}
		}

		[AllowAnonymous]
		[HttpPost]
		public IActionResult Update(MultiAssistDto payload) {
			try {

			} catch (System.Exception ex) {

				throw;
			}
		}
	}
}