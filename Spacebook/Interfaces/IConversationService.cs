using Spacebook.Models;

namespace Spacebook.Interfaces
{
	public interface IConversationService : IGenericService<Conversation>
	{
		new Conversation Add(Conversation entity);
	}
}
