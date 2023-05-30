using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
			});

			// secondly with the newPost Entity, use the PostId to insert into the comments table
			this.commentService.Add(new Comment
			{
				OriginalPost = originalPostId,
				CommentPost = commentPost.PostId,
			});

			return Ok();
		}
    }
}
