namespace Spacebook.Hubs
{
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.SignalR;
	using Spacebook.Data;
	using Spacebook.Interfaces;
	using Spacebook.Models;
	using System;

	public class ConnectionHub : Hub
	{
		private readonly SignInManager<SpacebookUser> signInManager;
		private readonly IMessageService messageService;

		public ConnectionHub
		(
			SignInManager<SpacebookUser> signInManager, 
			IMessageService messageService
		)
		{
			this.signInManager = signInManager;
			this.messageService = messageService;
		}

		public override Task OnConnectedAsync()
		{
			//var userId = this.signInManager.UserManager.Users.First();
			// Not exactly sure what this will produce but will need to get the logged in users profileId/username

			var userId = $"user-{ConnectedUsers.Users.Count}";


			ConnectedUsers.Users.Add(userId, $"{Context.ConnectionId}");
			Console.WriteLine($"Connected - {ConnectedUsers.Users.Last()}");

			return base.OnConnectedAsync();
		}

		public override Task OnDisconnectedAsync(Exception? exception)
		{
			var userId = ConnectedUsers.Users.FirstOrDefault(_ => _.Value == Context.ConnectionId).Key;

			if (userId != null)
			{
				ConnectedUsers.Users.Remove(userId);
			}
			
			return base.OnDisconnectedAsync(exception);
		}

		public async Task SendMessage(string user, string message)
		{
			await Clients.All.SendAsync("ReceiveMessage", user, message);
		}

		public async Task SendPrivateMessage(string user, string message, string toUser)
		{
			if (ConnectedUsers.Users.ContainsKey(toUser))
			{
				await Clients.Client(ConnectedUsers.Users[toUser]).SendAsync("ReceiveMessage", user, message);
			}
			else
			{
				var newMessage = new Message()
				{
					ConversationId = 1,
					SenderId = 1,
					MessageType = "Text",
					Content = message,
					Timestamp = DateTime.UtcNow,
					Seen = false
				};

				this.messageService.Add(newMessage);
			}
		}
	}
}
