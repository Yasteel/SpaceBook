namespace Spacebook.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using Spacebook.Interfaces;
    using Spacebook.Models;

    [Authorize]
    public class ProfileController : Controller
    {
        private readonly SignInManager<SpacebookUser> signInManager;
        private readonly UserManager<SpacebookUser> userManager;
        private readonly IProfileService profileService;
        private readonly IPostService postService;

		public ProfileController(SignInManager<SpacebookUser> signInManager, UserManager<SpacebookUser> userManager, IProfileService profileService, IPostService postService)
		{
			this.signInManager = signInManager;
			this.userManager = userManager;
			this.profileService = profileService;
			this.postService = postService;
		}

		public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            var profile = profileService.GetByEmail(user.Email);
            if (profile == null)
            {
                return NotFound();
            }

            return View(profile);
        }

        public async Task<IActionResult> ViewProfile(string email)
        {
            var profile = profileService.GetByEmail(email);
            if (profile == null)
            {
                return NotFound();
            }
            return View(profile);
        }

        public async Task<IActionResult> Edit()
        {
            var user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            var profile = profileService.GetByEmail(user.Email);

            if (profile == null)
            {
                return NotFound();
            }

            return View(profile);
        }
    }
}
