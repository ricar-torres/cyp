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
			List<MultiAssistDto> multiAssistDtoList = new List<MultiAssistDto>();
			try {
				var multiAssist = _service.GetAll().ToList();
				multiAssist.ForEach((m) => {
					if (m.Cover is Covers c) {
						var mdto = new MultiAssistDto {
							MultiAssist = m,
								Name = c.Name
						};
						mdto.MultiAssist.Cover = null;
						multiAssistDtoList.Add(mdto);
					}
				});
			} catch (System.Exception ex) {
				return DefaultError(ex);
			}
			return Ok(multiAssistDtoList);
		}

		[AllowAnonymous]
		[HttpGet("[action]")]
		public IActionResult GetMultiAssistPlans() {
			var list = _service.GetMultiAssistPlans().ToList();
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
		[HttpGet("{id}")]
		public IActionResult GetById(int id) {
			MultiAssistDto res = new MultiAssistDto();
			List<Beneficiaries> beneficiaryList;
			List<MultiAssistsVehicle> vehicleList;
			try {
				res.MultiAssist = _service.GetById(id);
				vehicleList = new List<MultiAssistsVehicle>(res.MultiAssist.MultiAssistsVehicle);
				beneficiaryList = new List<Beneficiaries>(res.MultiAssist.Beneficiaries);
				vehicleList.ForEach((v) => {
					v.MultiAssists = null;
				});
				beneficiaryList.ForEach((b) => {
					b.MultiAssists = null;
				});
				res.MultiAssist.MultiAssistsVehicle = vehicleList;
				res.MultiAssist.Beneficiaries = beneficiaryList;
				res.HealthPlan = res.MultiAssist.Cover.HealthPlan;
				res.MultiAssist.Cover.MultiAssists = null;
				res.HealthPlan.Covers = null;
				res.MultiAssist.Cover.HealthPlan = null;
				return Ok(res);
			} catch (System.Exception ex) {
				return DefaultError(ex);
			}
		}

		[AllowAnonymous]
		[HttpPost]
		public IActionResult Create([FromBody] MultiAssistDto payload) {

			try {
				int cpId;
				ClientProduct clientProduct = new ClientProduct {
					ClientId = payload.ClientId,
						ProductId = 2, //MULTI-ASSIST ID
						Status = 0,
						CreatedAt = DateTime.Now
				};

				cpId = _clientProductService.Create(clientProduct);

				if (cpId > 0) {
					payload.MultiAssist.ClientProductId = cpId;
					_service.Create(payload.MultiAssist);
					payload.MultiAssist.ClientProduct = null;
				} else {
					return BadRequest();
				}

				return Ok(payload);

			} catch (System.Exception ex) {
				return DefaultError(ex);
			}
		}

		[AllowAnonymous]
		[HttpPut]
		public IActionResult Update([FromBody] MultiAssistDto payload) {
			try {
				this._service.Update(payload.MultiAssist);
				return Ok();
			} catch (System.Exception ex) {
				return DefaultError(ex);
			}
		}

		[AllowAnonymous]
		[HttpDelete("{id}")]
		public IActionResult Remove(int id) {
			try {
				var mas = _service.GetById(id);
				mas.DeletedAt = DateTime.Now;
				this._service.Update(mas);
				return Ok();
			} catch (System.Exception ex) {
				return DefaultError(ex);
			}
		}
	}
}