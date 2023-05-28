using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Spacebook.Data;
using Spacebook.Interfaces;
using Spacebook.Models;

namespace Spacebook.WebApiController
{
	public class MessageWebApiController : Controller
	{
		private readonly IMessageService messageService;
		private readonly IConversationService conversationService;
		private readonly UserManager<SpacebookUser> userManager;
		private readonly IProfileService profileService;

		public MessageWebApiController
		(
			IMessageService messageService,
			IProfileService profileService,
			IConversationService conversationService,
			UserManager<SpacebookUser> userManager
		)
        {
			this.messageService = messageService;
			this.conversationService = conversationService;
			this.userManager = userManager;
			this.profileService = profileService;
		}

		[HttpPost]
        public IActionResult NewMessage(int conversationId, string messageType, string content, int senderId)
		{
			Console.WriteLine(content);

			var newMessage = new Message()
			{
				ConversationId = 1,
				SenderId = 1,
				MessageType = messageType,
				Content = content,
				Timestamp = DateTime.UtcNow,
				Seen = false
			};

			this.messageService.Add(newMessage);

			return this.Ok();
		}


		[HttpGet]
		public object GetContacts()
		{
			return JsonConvert.SerializeObject(this.profileService.GetAll());
		}

		[HttpGet]
		public async Task<object> GetMessages(string contactUsername)
		{
			var spacebookUser = (SpacebookUser)await this.userManager.GetUserAsync(User);
			var thisUser = spacebookUser.Email;

			var conversationId = this.GetConversation(contactUsername).Result;


			// this user does not have a chat history with the selected user - create a conversation between users
			if(conversationId.Count < 1)
			{
				var thisUserProfile = this.profileService.GetByUsername(thisUser);
				var contactProfile = this.profileService.GetByUsername(contactUsername);

				this.conversationService.Add(new Conversation
				{
					CreatedAt = DateTime.Now,
					ParticipantOne = thisUserProfile.UserId,
					ParticipantTwo = contactProfile.UserId
				});

				// since a conversation was just created, an empty message list is returned
				return JsonConvert.SerializeObject(new List<Message>());
			}

			// Finds messages that exist between the 2 users
			return JsonConvert.SerializeObject(this.messageService.GetByConversationId(conversationId[0]));
		}

		private async Task<List<int>> GetConversation(string contactUsername)
		{
			var conversations = conversationService.GetAll();
			var profiles = profileService.GetAll();

			var spacebookUser = (SpacebookUser)await this.userManager.GetUserAsync(User);
			var thisUser = spacebookUser.Email;

			// Gets Conversation id of chat between this user and selected user (user's chat selected on front-end)
			return conversations
				.Join(profiles, a => a.ParticipantOne, b => b.UserId, (a, b) => new { Conversation = a, ProfileB = b })
				.Join(profiles, ab => ab.Conversation.ParticipantTwo, c => c.UserId, (ab, c) => new { ab.Conversation, ab.ProfileB, ProfileC = c })
				.Where(abcp =>
					(abcp.ProfileB.Username == contactUsername && abcp.ProfileC.Username == thisUser)
					|| (abcp.ProfileC.Username == contactUsername && abcp.ProfileB.Username == thisUser))
				.Select(abcp => abcp.Conversation.ConversationId)
				.ToList();
		}

	}
}
