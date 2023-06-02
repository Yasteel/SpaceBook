namespace Spacebook.RecommendationEngine.Services
{
    using Spacebook.Models;
    using Spacebook.RecommendationEngine.Interfaces;
    public class UserVectorBuilder : IUserVectorBuilder
    {
        private IEnumerable<string?> allTagNames;
        private IEnumerable<Post> posts;
        private IEnumerable<HashTag> hashTags;
        private IEnumerable<Likes>? likes;
        private int userProfileId;

        public void LoadData(IEnumerable<Post> posts, IEnumerable<Likes> likes, IEnumerable<HashTag> hashTags, int userProfileId)
        {
            this.allTagNames = hashTags.Select(_ => _.HashTagText).Distinct();
            this.posts = posts;
            this.hashTags = hashTags;
            this.likes = likes;
            this.userProfileId = userProfileId;
        }

        public Dictionary<string, double> GetUserVector(
            Dictionary<int, IDictionary<string, double>> postVectorTable)
        {
            var userVector = BuildUserVector(postVectorTable);

            return userVector;
        }

        private Dictionary<string, double> BuildUserVector(Dictionary<int, IDictionary<string, double>> postVectorTable)
        {
            var userVector = new Dictionary<string, double>();
            var userPreferenceTable = CreateUserPreferenceTable();

            foreach (var tag in allTagNames)
            {
                userVector[tag] = getUserPreferenceScore(tag, postVectorTable, userPreferenceTable);
            }

            return userVector;
        }

        private double getUserPreferenceScore(
            string tag,
            Dictionary<int, IDictionary<string, double>> postVectorTable,
            Dictionary<int, int> userPreferences)
        {
            double sum = 0;

            foreach (var postVector in postVectorTable)
            {
                if (userPreferences[postVector.Key].Equals(1))
                {
                    sum += postVectorTable[postVector.Key][tag];
                }
            }
            return sum;
        }

        private Dictionary<int, int> CreateUserPreferenceTable()
        {
            var userPreferences = new Dictionary<int, int>();

            foreach (var post in posts)
            {
                userPreferences[post.PostId] = DoesUserLike(post.PostId);
            }

            return userPreferences;
        }

        private int DoesUserLike(int postId)
        {
            var post = posts.FirstOrDefault(_ => _.PostId == postId);
            Likes? like;
            int userPref = 0;

            if (likes != null)
            {
                like = likes.FirstOrDefault(_ => _.PostId == postId);
                if (like != null)
                {
                    if (like.ProfileId == userProfileId)
                    {
                        userPref = 1;
                    }
                }
            }

            if (post.ProfileId == userProfileId)
            {
                userPref = 1;
            }

            return userPref;
        }
    }
}