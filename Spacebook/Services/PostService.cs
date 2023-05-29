using Spacebook.Interfaces;
using Spacebook.Models;

namespace Spacebook.Services
{
	public class PostService : GenericService<Post>, IPostService
	{
		private readonly ApplicationDbContext context;

		public PostService
        (
            ApplicationDbContext context    
        )
			: base ( context )
        {
			this.context = context;
		}
    }
}
