namespace Spacebook.RecommendationEngine
{
    using System;
    
    using Spacebook.Interfaces;
    using Spacebook.Models;

    public class RecommendationService: IRecommdationService
    {
        private readonly IHashTagService hashTagService;
        private readonly IPostService postService;

        IEnumerable<string?> allTagNames;

        IEnumerable<Post> posts;
        IEnumerable<HashTag> hashTags;
        IEnumerable<Likes> likes;
        IEnumerable<Comment> comments;

        IDictionary<int, IDictionary<string, int>> postBinaryTable = new Dictionary<int, IDictionary<string, int>>();
        IDictionary<int, IDictionary<string, double>> postNormalizedTable = new Dictionary<int, IDictionary<string, double>>();

        Dictionary<int, int> userPreferences = new Dictionary<int, int>();

        int profileId;
        public RecommendationService(IHashTagService hashTagService,
                                     IPostService postService,
                                     ILikeService likeService,
                                     ICommentService commentService)
        {
            this.hashTagService = hashTagService;
            this.postService = postService;

            allTagNames = hashTagService.GetAll().Select(_ => _.HashTagText).Distinct();

            if (!allTagNames.Any()) { throw new Exception(); };

            posts = postService.GetAll();
            hashTags = hashTagService.GetAll();
            likes = likeService.GetAll();
            comments = commentService.GetAll();

        }

        public void BuildPostBinaryVectors(int profileId)
        {
            this.profileId = profileId;

            foreach ( var post in posts ) 
            {
                postBinaryTable[post.PostId] = GetBinaryVector(post.PostId);
            }

            GetTagFrequencies();
            GetNormalizedVectorTable();
        }

        public void GetTagFrequencies() 
        {
            Dictionary<string, int> frequencies = new Dictionary<string, int>();
            Dictionary<string, double> inverseFrequencies = new Dictionary<string, double>();

            int totalPosts = posts.Count();

            foreach (var tag in allTagNames)
            {
                var count = hashTags.Where(_ => _.HashTagText == tag).Count();
                frequencies[tag] = count;

                double inverseDF = Math.Log(totalPosts / count, 10.0);
                inverseFrequencies[tag] = inverseDF;

            }
        }

        private void GetNormalizedVectorTable()
        {
            foreach ( var binaryVector in postBinaryTable)
            {
                postNormalizedTable[binaryVector.Key] = GetNormalizedVector(binaryVector.Value);
            }
            
        }

        private IDictionary<string, double> GetNormalizedVector(IDictionary<string, int> binaryVector)
        {
            IDictionary<string, double> postNormailzedVector = new Dictionary<string, double>();

            var total = binaryVector.Sum(_ => _.Value);

            foreach ( var  vector in binaryVector)
            {
                if (total == 0)
                {
                    postNormailzedVector[vector.Key] = 0;
                    continue;
                }
                postNormailzedVector[vector.Key] =  vector.Value / Math.Sqrt(total);
            }
            
            return postNormailzedVector;
        }

        private IDictionary<string, int> GetBinaryVector(int postId)
        {
            IDictionary<string, int> postBinaryVector = new Dictionary<string, int>();

            var tagsForPost = hashTagService.FindAllByField("PostId", postId).Select(_ => _.HashTagText).ToHashSet();

            foreach ( var tag in allTagNames ) 
            {
                int value = 0;
                if (tagsForPost.Contains(tag))
                {
                    value = 1;
                }
                postBinaryVector[tag] = value;
            }

            return postBinaryVector;
        }

        private void BuildUserVector()
        {
            Dictionary<string, int> userVector = new Dictionary<string, int>();

            foreach ( var tag in allTagNames ) 
            {
                userVector[tag] = getUserPreferenceScore(tag);
            }
        }

        private int getUserPreferenceScore(string tag)
        {
            throw new NotImplementedException();
        }

        private Dictionary<int, int> CreateUserPreferenceTable()
        {
            foreach (var post in posts)
            {
                userPreferences[post.PostId] = DoesUserLike(post.PostId);
            }

            return userPreferences;
        }

        private int DoesUserLike(int postId)
        {
            var post = posts.FirstOrDefault(_ => _.PostId == postId);
            var like = likes.FirstOrDefault(_ => _.PostId == postId);
            

            int userPref = 0;

            if (post == null)
            {
                return 0;
            }

            if (post.ProfileId == this.profileId)
            {
                userPref = 1;
            }

            if (like != null)
            {
                if (like.ProfileId == this.profileId) 
                {
                    userPref = 1;
                }
            }

            return userPref;
        }
    }
}
