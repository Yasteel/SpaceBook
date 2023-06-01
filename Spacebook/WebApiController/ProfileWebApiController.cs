namespace Spacebook.WebApiController
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
	using Microsoft.IdentityModel.Tokens;

	using Newtonsoft.Json;

    using Spacebook.Interfaces;
    using Spacebook.Models;
    using Spacebook.Services;

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

        [HttpGet]
        public IActionResult GetById(int Id)
        {
            var profile = profileService.GetById(Id);

            if (profile != null)
            {
                return Ok(profile);
            }

            return BadRequest();
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

				if(profile.ProfilePicture.IsNullOrEmpty())
                {
                    profile.ProfilePicture = "https://payconnect.blob.core.windows.net/2c9aca41-40f7-4e88-b854-1e5d6ba0f234/2c9aca41-40f7-4e88-b854-1e5d6ba0f234d4e5741f-9baf-4361-b5e3-89f908715ead";
                }

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
        public async Task<IActionResult> Upload(Profile model)
        {
            if (model == null)
            {
                return this.BadRequest("{\"Error\":[\"Could not complete request. Invalid data.\"]}");
            }

            var user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                return BadRequest();
            }

            var userProfile = profileService.GetByEmail(user.Email);

            if (userProfile == null)
            { 
                return NotFound(); 
            }

            string? fileURI;

            if(model.ProfilePictureFile != null)
            {
                fileURI = storageService.UploadBlob(model.ProfilePictureFile, user.Id);
                userProfile.ProfilePicture = fileURI;
            }

            this.profileService.Update(userProfile);

            return Ok();

        }
    }
}
