namespace Spacebook.RecommendationEngine.Services
{
    using System;

    using Spacebook.Interfaces;
    using Spacebook.Models;
    using Spacebook.RecommendationEngine.Interfaces;

    public class RecommendationService : IRecommdationService
    {
        private readonly IHashTagService hashTagService;
        private readonly IPostService postService;
        private readonly ILikeService likeService;

        private readonly IVectorBuilder vectorBuilder;
        private readonly IUserVectorBuilder userVectorBuilder;

        private IEnumerable<string?>? allTagNames;
        private IEnumerable<Post>? posts;
        private IEnumerable<HashTag>? hashTags;
        private IEnumerable<Likes>? likes;

        public RecommendationService(IHashTagService hashTagService,
                                     IPostService postService,
                                     ILikeService likeService,
                                     IVectorBuilder vectorBuilder,
                                     IUserVectorBuilder userVectorBuilder)
        {
            this.hashTagService = hashTagService;
            this.postService = postService;
            this.likeService = likeService;
            this.vectorBuilder = vectorBuilder;
            this.userVectorBuilder = userVectorBuilder;
        }

        public IEnumerable<Post> GetPosts(int userProfileId)
        {
            BuildRepositories();

            allTagNames = hashTagService.GetAll()
                .Select(_ => _.HashTagText).ToHashSet();

            if (!allTagNames.Any() || posts == null)
            {
                return new List<Post>();

            };

            var predictions = GetPredictionValues(userProfileId);

            if (allPredicationsZero(predictions))
            {
                return new List<Post>();
            }

            var sortedPosts = GetSortedPosts(predictions);
            var filteredPosts = GetFilteredPosts(sortedPosts, userProfileId);

            return sortedPosts;
        }

        private List<Post> GetFilteredPosts(List<Post> sortedPosts, int userProfileId)
        {
            //remove users own posts
            sortedPosts.RemoveAll(_ => _.ProfileId == userProfileId);

            //remove all already liked posts
            if (likes != null)
            {
                var likedPosts = likes.Where(_ => _.ProfileId == userProfileId).Select(_ => _.PostId).ToList();
                sortedPosts.RemoveAll(_ => likedPosts.Contains(_.PostId));
            }
            return sortedPosts;
        }

        private List<Post> GetSortedPosts(Dictionary<int, double> predictionValues)
        {
            var sortedPosts = new List<Post>();
            var sortedPredictions = predictionValues.OrderByDescending(_ => _.Value).ToList();

            foreach (var item in sortedPredictions)
            {
                var post = posts.FirstOrDefault(_ => _.PostId == item.Key);
                sortedPosts.Add(post);
            }

            return sortedPosts;
        }

        private Dictionary<int, double> GetPredictionValues(int userProfileId)
        {
            vectorBuilder.LoadData(posts, likes, hashTags);
            userVectorBuilder.LoadData(posts, likes, hashTags, userProfileId);

            var postVectorTable = vectorBuilder.GetPostVectorTable();
            var userVector = userVectorBuilder.GetUserVector(postVectorTable);

            var predIctedValues = CreatePredictions(postVectorTable, userVector);

            return predIctedValues;
        }

        private Dictionary<int, double> CreatePredictions(Dictionary<int, IDictionary<string, double>> postVectorTable,
                                                          Dictionary<string, double> userVector)
        {
            var predictions = new Dictionary<int, double>();

            foreach (var postVector in postVectorTable)
            {
                double dotProduct = 0;

                foreach (var vector in postVector.Value)
                {
                    dotProduct += vector.Value * userVector[vector.Key];
                }
                predictions.Add(postVector.Key, dotProduct);
            }
            return predictions;
        }

        private void BuildRepositories()
        {
            this.posts = postService.GetAll().Where(_ => _.Type != "Comment"); ;
            this.hashTags = hashTagService.GetAll();
            this.likes = likeService.GetAll();
        }

        private bool allPredicationsZero(Dictionary<int, double> predications)
        {
            return predications.All(_ => _.Value == 0);
        }
    }
}