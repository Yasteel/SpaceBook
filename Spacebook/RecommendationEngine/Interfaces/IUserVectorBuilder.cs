namespace Spacebook.RecommendationEngine.Interfaces
{
    using Spacebook.Models;

    public interface IUserVectorBuilder
    {
        void LoadData(List<string> allTagNames,
                      List<Post> posts,
                      List<Likes> likes,
                      List<HashTag> hashTags,
                      int userProfileId);

        Dictionary<string, double> GetUserVector(
            Dictionary<int, IDictionary<string, double>> postVectorTable);
    }
}
