using System.Collections.Generic;
using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Website.GraphQL.GraphTypes;
using GraphQL;
using GraphQL.Types;

namespace Autobarn.Website.GraphQL.Queries {
	public sealed class VehicleQuery : ObjectGraphType {
		private readonly IAutobarnDatabase db;

		public VehicleQuery(IAutobarnDatabase db) {
			this.db = db;
			Field<ListGraphType<VehicleGraphType>>("Vehicles")
				.Description("Return all vehicles")
				.Resolve(GetAllVehicles);
			Field<VehicleGraphType>("Vehicle")
				.Description("Return a single vehicle")
				.Arguments(MakeNonNullStringArgument("registration", "The registration of the vehicle you are querying"))
				.Resolve(GetVehicle);
		}

		private object GetVehicle(IResolveFieldContext<object> context) {
			var registration = context.GetArgument<string>("registration");
			return db.FindVehicle(registration);
		}

		private QueryArgument MakeNonNullStringArgument(string name, string description) {
			return new QueryArgument<NonNullGraphType<StringGraphType>> {
				Name = name, Description = description
			};
		}

		private IEnumerable<Vehicle> GetAllVehicles(IResolveFieldContext<object> arg)
			=> db.ListVehicles();
	}
}
