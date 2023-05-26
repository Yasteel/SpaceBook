using Spacebook.Interfaces;
using Spacebook.Models;

namespace Spacebook.Services
{
	public class MessageService : GenericService<Message>, IMessageService
	{
        public MessageService(ApplicationDbContext context)
            : base(context)
        {   
        }
    }
}
