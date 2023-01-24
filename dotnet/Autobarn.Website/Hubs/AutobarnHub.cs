using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Autobarn.Website.Hubs {
	public class AutobarnHub : Hub {
		public async Task NotifyWebsiteOfVehiclePrice(string user, string message) {
			await Clients.All.SendAsync("DisplayPriceNotification", user, message);
		}
	}
}
