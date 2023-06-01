namespace Spacebook.RecommendationEngine.Interfaces
{
    using Spacebook.Models;

    public interface IUserVectorBuilder
    {
        void LoadData(IEnumerable<Post> posts, IEnumerable<Likes> likes, IEnumerable<HashTag> hashTags, int userProfileId);

        public Dictionary<string, double> GetUserVector(
            Dictionary<int, IDictionary<string, double>> postVectorTable);
    }
}
