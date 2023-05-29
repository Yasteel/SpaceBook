using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Spacebook.Data;
using Spacebook.Interfaces;
using Spacebook.Models;

namespace Spacebook.WebApiController
{
    public class ProfileWebApiController : Controller
    {
        private readonly UserManager<SpacebookUser> userManager;
        private readonly IProfileService profileService;

        public ProfileWebApiController
        (
            UserManager<SpacebookUser> userManager,
            IProfileService profileService
        )
        {
            this.userManager = userManager;
            this.profileService = profileService;
        }

        [HttpPost]
        public async Task<IActionResult> Put(Profile model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);

                if (user == null)
                {
                    return NotFound();
                };

                var profileInfo = this.profileService.GetByUsername(model.Username);

                profileInfo.Name = model.Name;
                profileInfo.Surname = model.Surname;
                profileInfo.Email = model.Email;
                profileInfo.Bio = model.Bio;
                profileInfo.ProfilePicture = model.ProfilePicture;
                profileInfo.Gender = model.Gender;
                profileInfo.BirthDate = model.BirthDate;
                profileInfo.JoinedDate = DateTime.UtcNow;


                this.profileService.Update(profileInfo);

                return this.RedirectToAction("Index", "Home");

            }
            else
            {
                this.ModelState.AddModelError(string.Empty, "Invalid registration attempt.");
            }

            return RedirectToAction("Edit", "Profile", model);
        }
    }
}
