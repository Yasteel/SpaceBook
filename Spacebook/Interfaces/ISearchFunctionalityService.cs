using Spacebook.Models;

namespace Spacebook.Interfaces
{
    public interface ISearchFunctionalityService 
    {
        List<Post> posts(string searchItem);
    }
}
