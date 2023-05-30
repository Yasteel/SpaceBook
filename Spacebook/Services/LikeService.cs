namespace Spacebook.Services
{
	using Spacebook.Interfaces;
	using Spacebook.Models;

	public class LikeService : GenericService<Likes>, ILikeService
	{
		private readonly ApplicationDbContext context;

		public LikeService(ApplicationDbContext context)
            : base(context)
        {
			this.context = context;
		}

        public int LikeCount(int postId)
        {
            return this.context.Likes.Count(_ => _.PostId == postId);
        }

    }
}
