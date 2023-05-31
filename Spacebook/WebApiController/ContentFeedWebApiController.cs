using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Spacebook.Interfaces;
using Spacebook.Models;
using Spacebook.ViewModel;

namespace Spacebook.WebApiController
{
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

			var contentFeed = new
			{
				Post = new
				{
					PostId = post.PostId,
					Type = post.Type,
					MediaUrl = post.MediaUrl,
					Caption = post.Caption,
					Timestamp = post.Timestamp.ToLongDateString(),
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
