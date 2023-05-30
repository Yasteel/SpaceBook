using Spacebook.Interfaces;
using Spacebook.Models;

namespace Spacebook.Services
{
	public class ConversationService : GenericService<Conversation>, IConversationService
	{
		private readonly ApplicationDbContext context;

		public ConversationService (ApplicationDbContext context)
            : base ( context )
        {
			this.context = context;
		}

		// Adds new functionality to return the Conversation in order to get the inserted Conversation Id
		public new Conversation Add(Conversation entity)
		{
			this.context.Conversation.Add(entity);
			this.context.SaveChanges();

			return entity;
        }
    }
}
