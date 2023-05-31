namespace Spacebook.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Spacebook.Interfaces;
    using Spacebook.Models;
    using Spacebook.ViewModel;

    [Authorize]
    public class HomeController : Controller
    {
        private readonly IPostService postService;
        private readonly IProfileService profileService;
        private readonly ICommentService commentService;
        private readonly ILikesService likesService;

        public HomeController(
                              IPostService postService,
                              IProfileService profileService,
                              ICommentService commentService,
                              ILikesService likesService)
        {
            this.postService = postService;
            this.profileService = profileService;
            this.commentService = commentService;
            this.likesService = likesService;
        }

        [Authorize]
        public IActionResult Index()
        {
            List<ContentFeed> contentFeeds = new List<ContentFeed>();

            var posts = postService.GetAll();

            foreach (var post in posts) 
            {
                var contentFeed = GetContentFeed(post);
                contentFeeds.Add(contentFeed);

            }

            List<ContentFeed> sortedContent = contentFeeds.OrderByDescending(_ => _.Post.Timestamp).ToList();
            return View(sortedContent);
        }

        private ContentFeed GetContentFeed(Post post) 
        {
            var comments = this.commentService.FindAllByField("OriginalValue", post.PostId);
            var likes = this.likesService.FindAllByField("PostId", post.PostId);
            var profile = this.profileService.GetById(post.ProfileId);

            var contentFeed = new ContentFeed()
            {
                Comments = comments,
                Likes = likes,
                Post = post,
                Profile = profile
            };

            return contentFeed;
        }
    }
}