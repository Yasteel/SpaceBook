using Spacebook.Models;

namespace Spacebook.RecommendationEngine.Interfaces
{
    public interface IRecommdationService
    {
        IEnumerable<Post> GetPosts(int userProfileId);
    }
}
