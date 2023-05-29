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
		private readonly IPostService postService;
		private readonly UserManager<SpacebookUser> userManager;
		private readonly IProfileService profileService;

		public MessageWebApiController
		(
			IMessageService messageService,
			IProfileService profileService,
			IConversationService conversationService,
			IPostService postService,
			UserManager<SpacebookUser> userManager
		)
        {
			this.messageService = messageService;
			this.conversationService = conversationService;
			this.postService = postService;
			this.userManager = userManager;
			this.profileService = profileService;
		}

		[HttpPost]
        public IActionResult NewMessage(int conversationId, int senderId, string messageType, string content)
		{
			Console.WriteLine(content);

			var newMessage = new Message()
			{
				ConversationId = conversationId,
				SenderId = senderId,
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
		public async Task<object> GetMessages(int conversationId)
		{
			var spacebookUser = (SpacebookUser)await this.userManager.GetUserAsync(User);
			var thisUser = spacebookUser.Email;

			// Finds messages that exist between the 2 users
			var messages = this.messageService.GetByConversationId(conversationId);

			var returnObj = new List<Object>();


			foreach (var message in messages)
			{
				if (message.MessageType == "Post")
				{
					var post = this.postService.GetById(Int32.Parse(message.Content!));
					var userProfile = this.profileService.GetById(post.ProfileId);

					returnObj.Add(new
					{
						Message = message,
						Post = post,
						UserProfile = new
						{
							UserId = userProfile.UserId,
							Username = userProfile.Username,
							FullName = $"{userProfile.Name} {userProfile.Surname}",
							ProfilePicture = userProfile.ProfilePicture,
						}
					});
				}
				else
				{
					returnObj.Add(new
					{
						Message = message,
					});
				}
			}
			return JsonConvert.SerializeObject(returnObj);
		}

		[HttpGet]
		public async Task<int> GetConversationId(string contactUsername)
		{
			var profiles = profileService.GetAll();

			var spacebookUser = (SpacebookUser)await this.userManager.GetUserAsync(User);
			var thisUser = spacebookUser.Email;

			// Gets Conversation id of chat between this user and selected user (user's chat selected on front-end)
			var conversationId = conversationService.GetAll()
				.Join(profiles, a => a.ParticipantOne, b => b.UserId, (a, b) => new { Conversation = a, ProfileB = b })
				.Join(profiles, ab => ab.Conversation.ParticipantTwo, c => c.UserId, (ab, c) => new { ab.Conversation, ab.ProfileB, ProfileC = c })
				.Where(abcp =>
					(abcp.ProfileB.Username == contactUsername && abcp.ProfileC.Username == thisUser)
					|| (abcp.ProfileC.Username == contactUsername && abcp.ProfileB.Username == thisUser))
				.Select(abcp => abcp.Conversation.ConversationId)
				.ToList();

			// this user does not have a chat history with the selected user - create a conversation between users
			if (conversationId.Count < 1)
			{
				var thisUserProfile = this.profileService.GetByUsername(thisUser);
				var contactProfile = this.profileService.GetByUsername(contactUsername);

				var newConversation = this.conversationService.Add(new Conversation
				{
					CreatedAt = DateTime.Now,
					ParticipantOne = thisUserProfile.UserId,
					ParticipantTwo = contactProfile.UserId
				});

				return newConversation.ConversationId;
			}

			return conversationId[0];
		}


		[HttpGet]
		//[Route("/GetPost/{postId}")]
		public Object GetPost(int postId)
		{
			var post = this.postService.GetById(postId);

			var userProfile = this.profileService.GetById(post.ProfileId);

			var postObject = JsonConvert.SerializeObject(new
			{
				Post = post, 
				UserProfile = new
				{
					UserId = userProfile.UserId,
					Username = userProfile.Username,
					FullName = $"{userProfile.Name} {userProfile.Surname}",
					ProfilePicture = userProfile.ProfilePicture,
				}
			});

			return Ok(postObject);
		}


		// For Post as a Message
		// -> Share button on posts
		// -> Share button will need a popup with contacts to select
		// -> each contact will need data attributes: 
		//		-> conversationId
		//		-> senderUsername
		//		-> content -> postId
	}
}
