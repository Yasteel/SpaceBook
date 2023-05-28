namespace Spacebook.Hubs
{
	using System;
	using System.Security.Claims;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.SignalR;

	using Spacebook.Data;
	using Spacebook.Interfaces;
	using Spacebook.Models;


	public class ConnectionHub : Hub
	{
		private readonly UserManager<SpacebookUser> userManager;
		private readonly IMessageService messageService;
		private readonly IProfileService profileService;
		private readonly IHttpContextAccessor httpContextAccessor;

		public ConnectionHub
		(
			UserManager<SpacebookUser> userManager,
			IMessageService messageService,
			IProfileService profileService,
			IHttpContextAccessor httpContextAccessor
		)
		{
			this.userManager = userManager;
			this.messageService = messageService;
			this.profileService = profileService;
			this.httpContextAccessor = httpContextAccessor;
		}

		public async override Task OnConnectedAsync()
		{
			var userClaim = this.httpContextAccessor.HttpContext!.User;
			var user = (SpacebookUser)await this.userManager.GetUserAsync(userClaim);

			ConnectedUsers.Users.Add(user.Email, $"{Context.ConnectionId}");
			Console.WriteLine($"Connected - {ConnectedUsers.Users.Last()}");

			await base.OnConnectedAsync();
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
			// Checks if the recipient is online, if not - adds message to the database
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
