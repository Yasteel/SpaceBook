namespace Spacebook.Interfaces
{
	using Spacebook.Models;

	public interface ILikeService : IGenericService<Likes>
	{
		public int LikeCount(int postId);
	}
}
