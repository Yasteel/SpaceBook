using Spacebook.Interfaces;
using Spacebook.Models;

namespace Spacebook.Services
{
	public class MessageService : GenericService<Message>, IMessageService
	{
		private readonly ApplicationDbContext context;

		public MessageService(ApplicationDbContext context)
            : base(context)
        {
			this.context = context;
		}

		public List<Message> GetByConversationId(int conversationId)
		{
			return this.context.Message.Where(_ => _.ConversationId == conversationId).ToList<Message>();
		}
	}
}
