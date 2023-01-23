using System;
using Autobarn.Data;
using Autobarn.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Autobarn.Website.Models;

namespace Autobarn.Website.Controllers.Api {

	[ApiController]
	[Route("api/[controller]")]
	public class VehiclesController : ControllerBase {
		private readonly IAutobarnDatabase db;

		public VehiclesController(IAutobarnDatabase db) {
			this.db = db;
		}

		private const int PAGE_SIZE = 10;
		[HttpGet]
		public IActionResult Get(char startsWith = 'a') {
			var items = db.ListVehicles().Where(v => v.ModelCode.ToLower().StartsWith(startsWith));
			var total = items.Count();
			// ReSharper disable once InconsistentNaming
			dynamic _links = new ExpandoObject();
			_links.self = new {
				href = $"/api/vehicles?firstLetter={startsWith}"
			};
			if (startsWith != 'a' && total != 0) {
				_links.previous = new {
					href = $"/api/vehicles?firstLetter={(char)(startsWith - 1)}"
				};
			}

			if (startsWith != 'z' && total != 0) {
				_links.next = new {
					href = $"/api/vehicles?firstLetter={(char) (startsWith + 1)}"
				};
			}
			var result = new {
				_links,
				items
			};
			return Ok(result);
		}

		[HttpGet("{reg}")]
		public IActionResult Get(string reg) {
			var vehicle = db.FindVehicle(reg);
			if (vehicle == default) return NotFound();
			return Ok(vehicle);
		}

		[HttpPut("{reg}")]
		public IActionResult Put(string reg, [FromBody] VehicleDto dto) {
			var vehicle = db.FindVehicle(reg);
			if (vehicle == default) {
				vehicle = new Vehicle() {
					Color = dto.Color,
					ModelCode = dto.ModelCode,
					Registration = dto.Registration,
					Year = dto.Year
				};
				db.CreateVehicle(vehicle);
				return Created($"/api/vehicles/{vehicle.Registration}", vehicle);
			} else {
				vehicle.Registration = dto.Registration;
				vehicle.Color = dto.Color;
				vehicle.Year = dto.Year;
				vehicle.ModelCode = dto.ModelCode;
				db.UpdateVehicle(vehicle);
				return Ok(vehicle);
			}
		}
	}
}
