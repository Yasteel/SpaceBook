namespace Spacebook.Interfaces
{
    using Spacebook.Models;
    public interface ICommentService: IGenericService<Comment>
    {
        public int GetCommentCount(int postId);

	}
}
