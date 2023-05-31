namespace Spacebook.Interfaces
{
	using Spacebook.Models;

	public interface ILikeService : IGenericService<Likes>
	{
		public int GetLikeCount(int postId);
	}
}
