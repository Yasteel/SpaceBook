namespace Spacebook.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using Spacebook.Models;

    [Authorize]
    public class ProfileController : Controller
    {
        private readonly SignInManager<SpacebookUser> signInManager;
        private readonly UserManager<SpacebookUser> userManager;

        public ProfileController(SignInManager<SpacebookUser> signInManager, UserManager<SpacebookUser> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(User);
            return View(user);
        }

        public async Task<IActionResult> Edit()
        {
            var user = await userManager.GetUserAsync(User);
            return View(user);
        }
    }
}
