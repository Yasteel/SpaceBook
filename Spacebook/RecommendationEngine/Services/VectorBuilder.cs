
namespace Spacebook.RecommendationEngine.Services
{
    using Spacebook.Models;
    using Spacebook.RecommendationEngine.Interfaces;

    public class VectorBuilder : IVectorBuilder
    {
        private IEnumerable<string?> allTagNames;
        private IEnumerable<Post> posts;
        private IEnumerable<HashTag> hashTags;
        private IEnumerable<Likes>? likes;

        public void LoadData(IEnumerable<Post> posts, IEnumerable<Likes> likes, IEnumerable<HashTag> hashTags)
        {
            this.allTagNames = hashTags.Select(_ => _.HashTagText).Distinct();
            this.posts = posts;
            this.hashTags = hashTags;
            this.likes = likes;
        }

        public Dictionary<int, IDictionary<string, double>> GetPostVectorTable()
        {
            var postBinaryTable = BuildPostBinaryVectorTable();
            var dfValues = GetDFValues();
            var idfValues = GetInverseDFValues(dfValues);
            var postVectorTable = GetNormalizedVectorTable(postBinaryTable, idfValues);

            return postVectorTable;
        }

        private Dictionary<string, int> GetDFValues()
        {
            var frequencies = new Dictionary<string, int>();

            foreach (var tag in allTagNames)
            {
                var count = hashTags.Where(_ => _.HashTagText == tag).Count();
                frequencies[tag] = count;
            }

            return frequencies;
        }

        private Dictionary<string, double> GetInverseDFValues(Dictionary<string, int> frequencies)
        {
            var inverseFrequencies = new Dictionary<string, double>();
            int totalPosts = posts.Count();

            foreach (var tag in allTagNames)
            {
                var count = hashTags.Where(_ => _.HashTagText == tag).Count();
                frequencies[tag] = count;

                double inverseDF = Math.Log(totalPosts / count, 10.0);
                inverseFrequencies[tag] = inverseDF;
            }
            return inverseFrequencies;
        }

        private Dictionary<int, IDictionary<string, int>> BuildPostBinaryVectorTable()
        {
            var postBinaryTable = new Dictionary<int, IDictionary<string, int>>();
            
            foreach (var post in posts)
            {
                postBinaryTable[post.PostId] = GetBinaryVector(post.PostId);
            }
            return postBinaryTable;
        }

        private IDictionary<string, int> GetBinaryVector(int postId)
        {
            var postBinaryVector = new Dictionary<string, int>();

            // get all HashTag's for a PostID
            // store the words and remove duplicates
            var tagsInPost = hashTags.Where(_ => _.PostId == postId).Select(_ => _.HashTagText).ToHashSet();

            foreach (var tag in allTagNames)
            {
                int value = 0;
                if (tagsInPost.Contains(tag))
                {
                    value = 1;
                }
                postBinaryVector[tag] = value;
            }
            return postBinaryVector;
        }

        private Dictionary<int, IDictionary<string, double>> GetNormalizedVectorTable(

            Dictionary<int, IDictionary<string, int>> postBinaryTable,
            Dictionary<string, double> inverseFrequencies)

        {
            var postNormalizedTable = new Dictionary<int, IDictionary<string, double>>();
            foreach (var binaryVector in postBinaryTable)
            {
                postNormalizedTable[binaryVector.Key] = GetNormalizedVector(binaryVector.Value, inverseFrequencies);
            }
            return postNormalizedTable;
        }

        private IDictionary<string, double> GetNormalizedVector(

            IDictionary<string, int> binaryVector,
            Dictionary<string, double> inverseFrequencies)
        {
            var postNormailzedVector = new Dictionary<string, double>();

            var total = binaryVector.Sum(_ => _.Value);

            foreach (var vector in binaryVector)
            {
                if (total == 0)
                {
                    postNormailzedVector[vector.Key] = 0;
                    continue;
                }
                postNormailzedVector[vector.Key] = vector.Value / Math.Sqrt(total) * inverseFrequencies[vector.Key];
            }
            return postNormailzedVector;
        }
    }
}
