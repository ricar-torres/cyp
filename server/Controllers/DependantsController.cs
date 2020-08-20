using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Dtos;
using server.Entities;
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
			IEnumerable<TypeOfRelationship> relationships;
			DependentDto dependent = null;
			try {
				relationships = _service.GetRelationTypes().ToList();
				_service.GetAllByClient(id).ToList().ForEach((item) => {
					if (item is Dependents d && d.Cover is Covers c && c.Dependents is object) {
						d.Cover.Dependents = null;
						if (c.HealthPlan is HealthPlans hp) {
							hp.Covers = null;
						}
						item = d;
					}
					dependent = new DependentDto(item);
					dependent.Relationship = relationships.Where(_ => _.Id == item.Relationship).FirstOrDefault() is TypeOfRelationship relationship ?
						relationship : new TypeOfRelationship();
				});
				if (dependent is object) {
					return Ok(dependent);
				} else {
					return NotFound();
				}
			} catch (Exception ex) {
				return DefaultError(ex);
			}
		}

		// POST: api/CommunicationMethod
		[AllowAnonymous]
		[HttpPost]
		public IActionResult Create([FromBody] Dependents payload) {
			try {
				//payload.Cover = null;
				// (payload as Dependents).Relationship = payload.Relationship.Id;
				//payload.Relationship = null;
				var res = _service.Create(payload);
				return Ok(res);

			} catch (AppException ex) {
				// return error message if there was an exception
				return DefaultError(ex);
			}
		}

		[AllowAnonymous]
		[HttpGet("[action]/{clientId:int}")]
		public IActionResult GetAllByClient(int clientId) {
			//List<DependentDto> dependentsDtoList = new List<DependentDto>();
			List<DependentDto> dependents;
			IEnumerable<TypeOfRelationship> relationships;
			try {
				dependents = new List<DependentDto>();
				relationships = _service.GetRelationTypes().ToList();
				var list = _service.GetAllByClient(clientId).ToList();
				list.ForEach((item) => {
					if (item is Dependents d && d.Cover is Covers c && c.Dependents is object) {
						d.Cover.Dependents = null;
						if (c.HealthPlan is HealthPlans hp) {
							hp.Covers = null;
						}
						item = d;
					}
					dependents.Add(new DependentDto(item));
					dependents.Last().Relationship = relationships.Where(_ => _.Id == item.Relationship).FirstOrDefault() is TypeOfRelationship relationship ?
						relationship : new TypeOfRelationship();
				});
				if (dependents is object) {
					return Ok(dependents);
				} else {
					return NotFound();
				}
			} catch (Exception ex) {
				return DefaultError(ex);
			}
		}

		[AllowAnonymous]
		[HttpGet("[action]")]
		public IActionResult GetRelationTypes() {
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

		[AllowAnonymous]
		[HttpPut]
		public IActionResult Put(Dependents dependent) {
			Dependents updatedDependent;
			try {

				updatedDependent = _service.Update(dependent);

				return Ok(updatedDependent);

			} catch (AppException ex) {
				// return error message if there was an exception
				return DefaultError(ex);
			}

		}

		[AllowAnonymous]
		[HttpDelete("{id}")]
		public IActionResult Delete(int id) {
			try {

				_service.Delete(id);

				return Ok();

			} catch (AppException ex) {
				// return error message if there was an exception
				return DefaultError(ex);
			}

		}

		[HttpGet("test")]
		public IActionResult GetCovers() {
			try {
				var res = this._service.GetCovers();
				return Ok(res);
			} catch (System.Exception) {

				throw;
			}
		}

		[HttpGet("test1")]
		public IActionResult GetHealthPlans() {
			try {
				var res = this._service.GetHealthPlans();
				return Ok(res);
			} catch (System.Exception) {

				throw;
			}
		}
	}
}