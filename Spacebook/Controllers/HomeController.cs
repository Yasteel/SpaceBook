namespace Spacebook.Controllers
{
    using System.Diagnostics;

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

        public HomeController(
                              IPostService postService,
                              IProfileService profileService)
        {
            this.postService = postService;
            this.profileService = profileService;

        }

        [Authorize]
        public IActionResult Index()
        {
            List<ContentFeed> contentFeeds = new List<ContentFeed>();

            var posts = postService.GetAll();
            

            return View(posts);
        }

       
    }
}