using Microsoft.AspNetCore.Mvc;
using Spacebook.Interfaces;
using Spacebook.Models;

namespace Spacebook.WebApiController
{
	public class MessageWebApiController : Controller
	{
		private readonly IMessageService messageService;

		public MessageWebApiController(IMessageService messageService)
        {
			this.messageService = messageService;
		}

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
	}
}
