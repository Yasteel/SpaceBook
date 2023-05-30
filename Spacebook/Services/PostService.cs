using Spacebook.Interfaces;
using Spacebook.Models;

namespace Spacebook.Services
{
    using Spacebook.Interfaces;
    using Spacebook.Models;
    public class PostService: GenericService<Post>, IPostService
    {
		private readonly ApplicationDbContext context;

		public PostService(ApplicationDbContext context)
            :base(context)
		{
			this.context = context;
		}

		public new Post Add(Post entity)
		{
			this.context.Post.Add(entity);
			this.context.SaveChanges();

			return entity;
		}
	}
}
