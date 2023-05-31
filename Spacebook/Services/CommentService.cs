namespace Spacebook.Services
{
    using Spacebook.Interfaces;
    using Spacebook.Models;
    public class CommentService: GenericService<Comment>, ICommentService
    {
		private readonly ApplicationDbContext context;

		public CommentService(ApplicationDbContext context)
            : base(context)
		{
			this.context = context;
		}

		public int GetCommentCount(int postId)
		{
			return this.context.Comment.Count(_ => _.OriginalPost == postId);
		}
	}
}
