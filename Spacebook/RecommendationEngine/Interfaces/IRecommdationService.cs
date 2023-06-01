using Spacebook.Models;

namespace Spacebook.RecommendationEngine.Interfaces
{
    public interface IRecommdationService
    {
        List<Post> GetPosts(int userProfileId);
    }
}
