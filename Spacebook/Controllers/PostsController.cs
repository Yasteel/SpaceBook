namespace Spacebook.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Spacebook.Interfaces;

    public class PostsController : Controller
    {
        private readonly IPostService postService;

        public PostsController(IPostService postService)
        {
            this.postService = postService;
        }

        [HttpGet]
        public async Task<IActionResult> ViewPost(string Id)
        {
            var post = postService.GetById(int.Parse(Id));

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }
    }
}
