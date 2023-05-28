using Spacebook.Models;

namespace Spacebook.Interfaces
{
	public interface IMessageService : IGenericService<Message>
	{
		List<Message> GetByConversationId(int conversationId);
	}
}
