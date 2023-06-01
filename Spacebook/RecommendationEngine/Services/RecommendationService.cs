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

        public List<Post> GetPosts(int userProfileId)
        {
            var allTagNames = hashTagService.GetAll()
                .Select(_ => _.HashTagText).Distinct();

            if (!allTagNames.Any())
            {
                return new List<Post>();

            };

            var predictions = GetPredictionValues(userProfileId);
            var sortedPosts = GetSortedPosts(predictions);

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



        public Dictionary<int, double> GetPredictionValues(int userProfileId)
        {
            BuildRepositories();

            var postVectorTable = vectorBuilder.GetPostVectorTable();
            var userVector = userVectorBuilder.GetUserVector(userProfileId, postVectorTable);

            Dictionary<int, double> predictions = new Dictionary<int, double>();

            foreach (var postVector in postNormalizedTable)
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
            posts = postService.GetAll();
            hashTags = hashTagService.GetAll();
            likes = likeService.GetAll();
        }
    }
}