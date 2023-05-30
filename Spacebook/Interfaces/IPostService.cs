namespace Spacebook.Interfaces
{
    using Spacebook.Models;
    public interface IPostService: IGenericService<Post>
    {
        new Post Add(Post entity);
    }
}
