using Spacebook.Models;

namespace Spacebook.RecommendationEngine.Interfaces
{
    public interface IVectorBuilder
    {
        void LoadData(List<Post> posts, List<Likes> likes, List<HashTag> hashTags);

        Dictionary<int, IDictionary<string, double>> GetPostVectorTable();

    }

}
