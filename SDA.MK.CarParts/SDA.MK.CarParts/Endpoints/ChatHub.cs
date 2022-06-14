using Microsoft.AspNetCore.SignalR;
using SDA.MK.CarParts.Models;

namespace SDA.MK.CarParts.Endpoints
{
	public class ChatHub : Hub
	{
		public async Task SendToAll(string name, string message, MessageType type)
		{
			await Clients.All.SendAsync("sendToAll", name, message, type);
		}
	}
}
