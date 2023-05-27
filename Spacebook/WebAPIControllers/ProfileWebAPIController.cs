namespace Spacebook.WebAPIControllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using Spacebook.Models;

    [Route("api/[controller]")]
    public class ProfileWebAPIController : Controller
    {
        private readonly UserManager<SpacebookUser> userManager;

        public ProfileWebAPIController(UserManager<SpacebookUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Put(SpacebookUser model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);

                if (user == null)
                {
                    return NotFound();
                }

                user.PhoneNumber = model.PhoneNumber;
                user.Email = model.Email;
                user.Bio = model.Bio;
                user.BirthDate = model.BirthDate;
                user.DisplayName = model.DisplayName;
                user.Gender = model.Gender;
                user.ProfilePicture = model.ProfilePicture;

                var result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Edit","Profile", model);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            else
            {
                this.ModelState.AddModelError(string.Empty, "Invalid registration attempt.");
            }

            return RedirectToAction("Edit", "Profile", model);
        }
    }
}
