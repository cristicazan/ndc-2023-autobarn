using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Autobarn.Website.Hubs {
	public class AutobarnHub : Hub {
		public async Task ThisIsMagicStringNumberOne(string user, string message) {
			await Clients.All.SendAsync("ThisIsMagicStringNumberTwo", user, message);
		}
	}
}
