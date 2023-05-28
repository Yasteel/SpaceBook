using Spacebook.Interfaces;
using Spacebook.Models;

namespace Spacebook.Services
{
	public class ConversationService : GenericService<Conversation>, IConversationService
	{
        public ConversationService (ApplicationDbContext context)
            : base ( context )
        {
        }
    }
}
