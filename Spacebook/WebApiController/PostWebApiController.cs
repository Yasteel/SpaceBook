using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Spacebook.Interfaces;
using Spacebook.Models;
using Spacebook.Services;

namespace Spacebook.WebApiController
{
    [Route("api/[controller]")]
    public class PostWebApiController : Controller
    {
        private readonly UserManager<SpacebookUser> userManager;
        private readonly IPostService postService;

        public PostWebApiController(UserManager<SpacebookUser> userManager, IPostService postService)
        {
            this.userManager = userManager;
            this.postService = postService;
        }

        [HttpGet]
        public IActionResult GetByProfileId(int Id)
        {
            var posts = postService.FindAllByField("ProfileId", Id);

            if(posts != null)
            {
                return Ok(posts);
            }

            return Ok();
        }
    }
}
