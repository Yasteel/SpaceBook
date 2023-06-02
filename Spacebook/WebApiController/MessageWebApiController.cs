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
		private readonly IAzureBlobStorageService storageService;

		public MessageWebApiController
		(
			IMessageService messageService,
			IProfileService profileService,
			IConversationService conversationService,
			IPostService postService,
			UserManager<SpacebookUser> userManager
,
			IAzureBlobStorageService storageService)
		{
			this.messageService = messageService;
			this.conversationService = conversationService;
			this.postService = postService;
			this.userManager = userManager;
			this.profileService = profileService;
			this.storageService = storageService;
		}

		[HttpGet]
		public object GetContacts()
		{
			return JsonConvert.SerializeObject(this.profileService.GetAll());
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
					(abcp.ProfileB.Email == contactUsername && abcp.ProfileC.Email == thisUser)
					|| (abcp.ProfileC.Email == contactUsername && abcp.ProfileB.Email == thisUser))
				.Select(abcp => abcp.Conversation.ConversationId)
				.ToList();

			// this user does not have a chat history with the selected user - create a conversation between users
			if (conversationId.Count < 1)
			{
				var thisUserProfile = this.profileService.GetByEmail(thisUser);
				var contactProfile = this.profileService.GetByEmail(contactUsername);

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
		public async Task<object> GetMessages(int conversationId)
		{
			var spacebookUser = (SpacebookUser)await this.userManager.GetUserAsync(User);
			var thisUser = this.profileService.GetByEmail(spacebookUser.Email);
			

			// Finds messages that exist between the 2 users
			var messages = this.messageService.GetByConversationId(conversationId);

			var returnObj = new List<Object>();


			foreach (var message in messages)
			{
				if (thisUser.UserId != message.SenderId)
				{
					message.Seen = true;
					this.UpdateMessageStatus(message);
				}

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
							Username = userProfile.Email,
							FullName = userProfile.DisplayName,
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
					FullName = userProfile.DisplayName,
					ProfilePicture = userProfile.ProfilePicture,
				}
			});

			return Ok(postObject);
		}

		[HttpPost]
		public async Task<IActionResult> Upload(Message model)
		{
			if (model == null)
			{
				return this.BadRequest("{\"Error\":[\"Could not complete request. Invalid data.\"]}");
			}

			var user = await userManager.GetUserAsync(User);

			if (user == null)
			{
				return BadRequest();
			}

			string? fileURI;

			if (model.MessageImage != null)
			{
				fileURI = storageService.UploadBlob(model.MessageImage, user.Id);

				var url = "{\"url\": \"" + fileURI + "\"}";

				return Ok(url);
			}

			var noImage = "{\"status\": \"No Image\"}";

			return Ok(noImage);

		}

		[HttpPost]
		public void UpdateMessageStatus(Message message)
		{
			this.messageService.Update(message);
		}
	}
}
