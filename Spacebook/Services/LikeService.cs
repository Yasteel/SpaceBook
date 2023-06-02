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

        public int GetLikeCount(int postId)
        {
            return this.context.Likes.Count(_ => _.PostId == postId);
        }

		public string GetEmail(int postId) 
		{
			var fkProfileId = this.GetProfileId(postId);
			var email = this.email(fkProfileId);

			return email;
		}

		private int GetProfileId(int postId) 
		{
            int? num = context.Post
                .Where(v1 => v1.PostId == postId)
				.Select(v1 => v1.ProfileId)
				.SingleOrDefault();

			return (int)num;
            
		}

		private string email(int postId) 
		{
			string? email = context.Profile
				.Where(v2 => v2.UserId == postId)
				.Select(v2 => v2.Email)
                .SingleOrDefault();
				
			return email;
		}




    }
}
