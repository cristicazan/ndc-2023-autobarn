using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Website.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Autobarn.Website.Controllers.Api {

	[ApiController]
	[Route("api/[controller]")]
	public class ModelsController : ControllerBase {

		private readonly IAutobarnDatabase db;

		public ModelsController(IAutobarnDatabase db) {
			this.db = db;
		}

		[HttpGet]
		public IEnumerable<Model> Get() {
			return db.ListModels();
		}

		[HttpGet("{code}")]
		public IActionResult Get(string code) {
			var model = db.FindModel(code);
			if (model == default) return NotFound();
			return Ok(model);
		}

		[HttpPost]
		public IActionResult Post([FromBody] ModelDto dto) {
			var model = db.FindModel(dto.Code);
			if (model == default) {
				model = new Model() {
					ManufacturerCode = dto.ManufacturerCode,
					Name = dto.Name,
					Code = dto.Code
				};
				db.CreateModel(model);
				return Created($"/api/models/{model.Code}", model);
			} else {
				model.ManufacturerCode = dto.ManufacturerCode;
				model.Name = dto.Name;
				model.Code = dto.Code;
				db.UpdateModel(model);
				return Ok(model);
			}
		}
	}
}
