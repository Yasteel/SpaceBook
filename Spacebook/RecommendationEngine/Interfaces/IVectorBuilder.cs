using Spacebook.Models;

namespace Spacebook.RecommendationEngine.Interfaces
{
    public interface IVectorBuilder
    {
        void LoadData(IEnumerable<Post> posts, IEnumerable<Likes> likes, IEnumerable<HashTag> hashTags);

        Dictionary<int, IDictionary<string, double>> GetPostVectorTable();

    }

}
