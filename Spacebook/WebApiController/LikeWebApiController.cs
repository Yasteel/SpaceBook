using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using OPENAI.Data;
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
		private readonly IHubContext<NotificationHub> _hubContext;
		private readonly INotificationService notificationService;
		

		public LikeWebApiController
        (
            ILikeService likeService,
			IPostService postService,
			IProfileService profileService,
			UserManager<SpacebookUser> userManager,
			IHubContext<NotificationHub> hubContext,
			INotificationService notificationService
        )
        {
			this.likeService = likeService;
			this.postService = postService;
			this.profileService = profileService;
			this.userManager = userManager;
			this._hubContext = hubContext;
			this.notificationService = notificationService;
		}

		[HttpPost]
		public async Task<object> Like(int postId)
		{
			bool success;

			var spacebookUser = (SpacebookUser)await this.userManager.GetUserAsync(User);
			var thisUserProfile = this.profileService.GetByEmail(spacebookUser.Email);

			this.likeService.Add(new Likes
			{
				PostId = postId,
				ProfileId = thisUserProfile.UserId,
				Timestamp = DateTime.UtcNow,
			});

            var email = this.likeService.GetEmail(postId);
			var username = this.profileService.GetByUsername(spacebookUser.UserName);
			//await _hubContext.Clients.User(email).SendAsync(spacebookUser + " has like your post");

			//this.likeService.Add(new Notification
			//             {
			//                 Username = email,
			//                 NotificationText = spacebookUser + " has liked your post"
			//             });

			this.notificationService.Add(new Notification
			{
                Username = email,
                NotificationText = username + " has liked your post"
            });

			//try
			//{
   //             await _hubContext.Clients.User(email).SendAsync(spacebookUser + " has like your post");
			//	success = true;

   //         }
			//catch (Exception ex) 
			//{
			//	success = false;	
			//}

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
