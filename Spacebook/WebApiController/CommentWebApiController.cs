using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Spacebook.Interfaces;
using Spacebook.Models;

namespace Spacebook.WebApiController
{
	public class CommentWebApiController : Controller
	{
		private readonly ICommentService commentService;
		private readonly IPostService postService;
		private readonly IProfileService profileService;
		private readonly UserManager<SpacebookUser> userManager;

		public CommentWebApiController
		(
			ICommentService commentService,
			IPostService postService,
			IProfileService profileService,
			UserManager<SpacebookUser> userManager
		)
		{
			this.commentService = commentService;
			this.postService = postService;
			this.profileService = profileService;
			this.userManager = userManager;
		}

		[HttpPost]
		public async Task<object> MakeComment(int originalPostId, string comment)
		{
			var spacebookUser = (SpacebookUser)await this.userManager.GetUserAsync(User);
			var thisUserProfile = this.profileService.GetByEmail(spacebookUser.Email);

			// First thing is to make a post with a type -> "Comment"
			var commentPost = this.postService.Add(new Post
			{
				ProfileId = (int)thisUserProfile.UserId!,
				Type = "Comment",
				Caption = comment,
				Timestamp = DateTime.UtcNow,
				AccessLevel = "Public"
			});

			// secondly with the newPost Entity, use the PostId to insert into the comments table
			this.commentService.Add(new Comment
			{
				OriginalPost = originalPostId,
				CommentPost = commentPost.PostId,
			});

			return Ok();
		}

		[HttpGet]
		public object GetCommentsForPost(int originalPostId)
		{
			var commentList = this.commentService.GetAll().Where(_ => _.OriginalPost == originalPostId);
			var commentPosts = new List<object>();

			foreach (var comment in commentList)
			{
				var post = this.postService.GetById((int)comment.CommentPost!);

				var profile = this.profileService.GetById(post.ProfileId);


				commentPosts.Add(new
				{
					Profile = new
					{
						DisplayName = profile.DisplayName,
						Email = profile.Email,
					},
					Post = new
					{
						PostId = post.PostId,
						Caption = post.Caption,
						Timestamp = post.Timestamp.ToLongDateString(),
					}
				});
			}

			return JsonConvert.SerializeObject(commentPosts);
		}

		[HttpGet]
		public int GetCommentCount(int postId)
		{
			return this.commentService.GetCommentCount(postId);
		}
	}
}
