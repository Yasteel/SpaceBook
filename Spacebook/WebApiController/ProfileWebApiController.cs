﻿namespace Spacebook.WebApiController
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using Newtonsoft.Json;

    using Spacebook.Interfaces;
    using Spacebook.Models;

    [Route("api/[controller]")]
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
        public async Task<IActionResult> Put(string values)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);

                if (user == null)
                {
                    return NotFound();
                }

                var profile = profileService.GetByEmail(user.Email);

                JsonConvert.PopulateObject(values, profile);

                profileService.Update(profile);

                return RedirectToAction("Edit", "Profile", profile);
            }
            else
            {
                this.ModelState.AddModelError(string.Empty, "Invalid registration attempt.");
            }

            return RedirectToAction("Edit", "Profile");
        }
    }
}
