using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Spacebook.Interfaces;
using Spacebook.Models;

namespace Spacebook.WebApiController
{
	public class LikeWebApiController : Controller
	{
		private readonly ILikeService likeService;
		private readonly IPostService postService;
		private readonly IProfileService profileService;
		private readonly UserManager<SpacebookUser> userManager;

		public LikeWebApiController
        (
            ILikeService likeService,
			IPostService postService,
			IProfileService profileService,
			UserManager<SpacebookUser> userManager
        )
        {
			this.likeService = likeService;
			this.postService = postService;
			this.profileService = profileService;
			this.userManager = userManager;
		}

		[HttpPost]
		public async Task<object> Like(int postId)
		{
			var spacebookUser = (SpacebookUser)await this.userManager.GetUserAsync(User);
			var thisUserProfile = this.profileService.GetByEmail(spacebookUser.Email);

			this.likeService.Add(new Likes
			{
				PostId = postId,
				ProfileId = thisUserProfile.UserId,
				Timestamp = DateTime.UtcNow,
			});

			return Ok();
		}

		[HttpPost]
		public async Task<object> Unlike(int postId)
		{
			var spacebookUser = (SpacebookUser)await this.userManager.GetUserAsync(User);
			var thisUserProfile = this.profileService.GetByEmail(spacebookUser.Email);

			var likeEntity = this.likeService.GetAll().Where(_ => _.PostId == postId && _.ProfileId == thisUserProfile.UserId);

			if(likeEntity.Any())
			{
				this.likeService.Delete(likeEntity.FirstOrDefault()!.LikeId);
			}

			return Ok();
		}

		[HttpGet]
		public int GetLikeCount(int postId)
		{
			return this.likeService.GetLikeCount(postId);
		}
	}
}
