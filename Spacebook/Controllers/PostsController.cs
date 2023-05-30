using Microsoft.AspNetCore.Mvc;
using Spacebook.Interfaces;
using Spacebook.Services;

namespace Spacebook.Controllers
{
    public class PostsController : Controller
    {
        private readonly IPostService postService;

        public PostsController(IPostService postService)
        {
            this.postService = postService;
        }

        [HttpGet]
        public async Task<IActionResult> ViewPost(string PostId)
        {
            var post = postService.GetById(int.Parse(PostId));

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }
    }
}
