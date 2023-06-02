namespace Spacebook.WebApiController
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using Spacebook.Interfaces;
    using Spacebook.Models;
    public class ContentFeedWebApiController : Controller
	{
		private readonly IPostService postService;
		private readonly IProfileService profileService;
		private readonly ICommentService commentService;
		private readonly ILikeService likeService;
        private readonly UserManager<SpacebookUser> userManager;

        public ContentFeedWebApiController
        (
			IPostService postService,
			IProfileService profileService,
			ICommentService commentService,
			ILikeService likeService,
            UserManager<SpacebookUser> userManager
        )
        {
			this.postService = postService;
			this.profileService = profileService;
			this.commentService = commentService;
			this.likeService = likeService;
            this.userManager = userManager;
        }

		public async Task<object> Get()
		{
            var spacebookUser = (SpacebookUser)await this.userManager.GetUserAsync(User);
            var thisUserProfile = this.profileService.GetByEmail(spacebookUser.Email);

            List<object> contentFeeds = new List<object>();

			var posts = postService.GetAll().Where(_ => _.Type != "Comment");
            posts = posts.OrderByDescending(_ => _.Timestamp);


            foreach (var post in posts)
			{
				var contentFeed = GetPostInfo(post, thisUserProfile.UserId);
				contentFeeds.Add(contentFeed);
			}

			return JsonConvert.SerializeObject(contentFeeds);
		}

		private object GetPostInfo(Post post, int? thisUserId)
		{
			var commentCount = this.commentService.GetCommentCount(post.PostId);
			var likeCount = this.likeService.GetLikeCount(post.PostId);
			var profile = this.profileService.GetById(post.ProfileId);

			var likedPost = this.likeService.GetAll().Where(_ => _.PostId == post.PostId && _.ProfileId == thisUserId);

			var time = "";
			var minutes = 0;
			var hours = (DateTime.Now - post.Timestamp).Hours;
			minutes = (DateTime.Now - post.Timestamp).Minutes;

			if (hours < 1)
			{
				time = "Posted " + minutes + " minutes ago";
			}
			else if (hours >= 1 && hours <= 24)
			{
				time = "Posted " + hours + " hours and " + minutes + " minutes ago";
			}
			else
			{
				time = "Posted on " + post.Timestamp.ToString("dd/MM/yyyy");
			}


			var contentFeed = new
			{
				Post = new
				{
					PostId = post.PostId,
					Type = post.Type,
					MediaUrl = post.MediaUrl,
					Caption = post.Caption,
					Timestamp = time,
					AccessLevel = post.AccessLevel,
					SharedID = post.SharedIDs,
                    CommentCount = commentCount,
                    LikeCount = likeCount,
					
                },
                Profile = profile,
				LikedPost = likedPost.Any() ? true : false,
            };

			return contentFeed;
		}
	}
}
