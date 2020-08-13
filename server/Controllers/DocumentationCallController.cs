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
	public class DocumentationCallController : BaseController {
		readonly IDocumentationCallService _service;
		readonly DataContext _context;
		readonly IMapper _mapper;
		public DocumentationCallController(IDocumentationCallService service, DataContext context, IMapper mapper) {
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

			try {
				payload.ConfirmationNumber = payload.ConfirmationNumber.Contains("000000000000") ?
					CreateConfirmationCode() : payload.ConfirmationNumber;
				_service.Create(payload);
				return Ok(payload);

			} catch (AppException ex) {
				// return error message if there was an exception
				return DefaultError(ex);
			}
		}

		private string CreateConfirmationCode() {
			var date = DateTime.Now;
			return date.ToString("yy") +
				date.ToString("MM") +
				date.ToString("dd") +
				date.ToString("hh") +
				date.ToString("mm") +
				date.ToString("ss");

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

		[AllowAnonymous]
		[HttpGet("[action]/{clientId:int}")]
		public IActionResult GetClientCalls(int clientId) {
			List<DocCallThread> masterThreads = new List<DocCallThread>();
			List<CallReasons> callReasons = _context.CallReasons.ToList();
			try {
				var table = _context.ClientUser.Where(x => x.ClientId == clientId).ToList();
				var res =
					from row in table
				group row by row.ConfirmationNumber into ConfirmationNumbers
				orderby ConfirmationNumbers.Key
				select ConfirmationNumbers;

				foreach (var nameGroup in res) {
					var masterThread = nameGroup.FirstOrDefault();
					masterThreads.Add(new DocCallThread() {
						Id = masterThread.Id,
							ConfirmationNumber = nameGroup.Key,
							CreatedAt = masterThread.CreatedAt.Value,
							Comment = masterThread.Comments,
							CallType = callReasons.FirstOrDefault(x => x.Id == masterThread.CallType).Name,
							Threads = new List<DocCallThread>()
					});
					foreach (var confirmationNumberGroup in nameGroup.Skip(1)) {
						masterThreads.Last().Threads.Add(
							new DocCallThread {
								Id = confirmationNumberGroup.Id,
									ConfirmationNumber = confirmationNumberGroup.ConfirmationNumber,
									Comment = confirmationNumberGroup.Comments,
									CreatedAt = confirmationNumberGroup.CreatedAt.Value,
									CallType = callReasons.FirstOrDefault(x => x.Id == confirmationNumberGroup.CallType).Name
							});
					}
				}

				if (res == null) {
					return NotFound();
				} else {
					return Ok(masterThreads);
				}

			} catch (Exception ex) {
				return DefaultError(ex);
			}
		}
	}
}