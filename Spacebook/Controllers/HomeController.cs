namespace Spacebook.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class HomeController : Controller
    {

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