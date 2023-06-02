using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Spacebook.Interfaces;
using Spacebook.Models;

namespace Spacebook.Controllers
{
    public class MessagesController : Controller
    {
        private readonly UserManager<SpacebookUser> userManager;
        private readonly IProfileService profileService;

        public MessagesController
        (
            UserManager<SpacebookUser> userManager,
            IProfileService profileService
        )
        {
            this.userManager = userManager;
            this.profileService = profileService;
        }


        public async Task<IActionResult> Index()
        {
            var spacebookUser = (SpacebookUser)await this.userManager.GetUserAsync(User);
            var thisUser = spacebookUser.Email;

            var userProfile = this.profileService.GetByEmail(thisUser);

            return View(userProfile);
        }
    }
}
