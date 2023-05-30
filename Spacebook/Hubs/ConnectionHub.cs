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
		private readonly IConversationService conversationService;
		private readonly IHttpContextAccessor httpContextAccessor;

		public ConnectionHub
		(
			UserManager<SpacebookUser> userManager,
			IMessageService messageService,
			IProfileService profileService,
			IConversationService conversationService,
			IHttpContextAccessor httpContextAccessor
		)
		{
			this.userManager = userManager;
			this.messageService = messageService;
			this.profileService = profileService;
			this.conversationService = conversationService;
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

		public async Task SendMessage(int conversationId, string senderUsername, string messageType, string content, string url)
		{
			var conversation = this.conversationService.GetById(conversationId);
			var senderId = this.profileService.GetByEmail(senderUsername).UserId;
			// toUser is the user that the message is being sent to / the recipient
			int toUser = (int)(conversation.ParticipantOne == senderId ? conversation.ParticipantTwo : conversation.ParticipantOne)!;

			// toUserEmail is used to get the connectionId from the Dictionary
			var toUserEmail = this.profileService.GetById(toUser).Email!;


			// Checks if recipient is also the sender or if user is not online -> adds message directly to database
			if (senderId == toUser || !ConnectedUsers.Users.ContainsKey(toUserEmail))
			{

				var newMessage = new Message()
				{
					ConversationId = conversationId,
					SenderId = senderId,
					MessageType = messageType,
					Content = content,
					MessageImageUrl = url,
					Timestamp = DateTime.UtcNow,
					Seen = false
				};

				this.messageService.Add(newMessage);
			}
			else
			{
				await Clients.Client(ConnectedUsers.Users[toUserEmail]).SendAsync("ReceiveMessage", conversationId, senderId, messageType, content, url);
			}
		}
	}
}
