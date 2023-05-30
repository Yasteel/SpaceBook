namespace Spacebook.Services
{
	using Spacebook.Interfaces;
	using Spacebook.Models;

	public class CommentService : GenericService<Comment>, ICommentService
	{
        public CommentService(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}
