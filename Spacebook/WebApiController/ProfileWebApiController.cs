namespace Spacebook.WebApiController
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Hosting;

    using Newtonsoft.Json;

    using Spacebook.Interfaces;
    using Spacebook.Models;

    [Route("api/[controller]")]
    public class ProfileWebApiController : Controller
    {
        private readonly UserManager<SpacebookUser> userManager;
        private readonly IProfileService profileService;
        private readonly IAzureBlobStorageService storageService;

        public ProfileWebApiController
        (
            UserManager<SpacebookUser> userManager,
            IProfileService profileService,
            IAzureBlobStorageService storageService)
        {
            this.userManager = userManager;
            this.profileService = profileService;
            this.storageService = storageService;
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

        [HttpPost("Upload")]
        public IActionResult Upload(Profile model)
        {
            if (model == null)
            {
                return this.BadRequest("{\"Error\":[\"Could not complete request. Invalid data.\"]}");
            }

            string userId = userManager.GetUserId(User);

            string? fileURI;

            if(model.ProfilePictureFile != null)
            {
                fileURI = storageService.UploadBlob(model.ProfilePictureFile, userId);
                model.ProfilePicture = fileURI;
            }

            this.profileService.Update(model);

            return Ok();

        }
    }
}
