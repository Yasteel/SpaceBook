namespace Spacebook.WebApiController
{
    using System.Text.RegularExpressions;

    using FluentValidation;
    using FluentValidation.Results;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using Microsoft.AspNetCore.Authorization;

    using Spacebook.Data;
    using Spacebook.Interfaces;
    using Spacebook.Models;

    [Authorize]
    public class CreatePostApiController: ControllerBase
    {
        private readonly IAzureBlobStorageService storageService;
        private readonly IPostService postService;
        private readonly UserManager<SpacebookUser> userManager;
        private readonly IHashTagService hashTagService;
        private readonly IValidator<Post> validator;
        private readonly IProfileService profileService;
        private readonly ISharedPostService sharedPostService;

        public CreatePostApiController(IAzureBlobStorageService storageService,
                               IPostService postService,
                               IProfileService profileService,
                               UserManager<SpacebookUser> userManager,
                               IHashTagService hashTagService,
                               IValidator<Post> validator,
                               ISharedPostService sharedPostService)
        {
            this.storageService = storageService;
            this.postService = postService;
            this.userManager = userManager;
            this.hashTagService = hashTagService;
            this.validator = validator;
            this.profileService = profileService;
            this.sharedPostService = sharedPostService;
        }

        [HttpPost]
        [Route("api/CreatePostApi/Post")]
        
        public IActionResult Post(Post model)
        {
            if (model == null)
            {
                return this.BadRequest("{\"Error\":[\"Could not complete request. Invalid data.\"]}");
            }

            ValidationResult result = this.validator.Validate(model);

            if (!result.IsValid)
            {
                return this.BadRequest(result.Errors);
            }

            if (!this.AllProfilesExist(model.SharedIDs))
            {
                return this.BadRequest("{\"Error\":[\"Could not complete request. Invalid data.\"]}");
            }

            string userId = userManager.GetUserId(User);

            // use this to save to database
            string? fileURI;
            if (model.ImageFile != null) 
            {
                fileURI = storageService.UploadBlob(model.ImageFile, userId);
                model.MediaUrl = fileURI;
                model.Type = model.ImageFile.ContentType;
            }
            else if (model.VideoFile != null)
            {
                fileURI = storageService.UploadBlob(model.VideoFile, userId);
                model.MediaUrl = fileURI;
                model.Type = model.VideoFile.ContentType;
            }

            model.Timestamp = DateTime.Now;

            //TODO get profile ID
            model.ProfileId = 1;

            this.postService.Add(model);

            SaveHashTags(model.Caption, model.PostId);
            SaveSharedPosts(model.SharedIDs, model.ProfileId);

            return Ok();
        }

        private void SaveHashTags(string caption, int id)
        {
            var tags = Regex.Split(caption, @"\s+").Where(i => i.StartsWith("#"));
            foreach (var tag in tags)
            {
                var hashTag = new HashTag()
                {
                    PostId = id,
                    HashTagText = tag.Substring(1),
                };

                this.hashTagService.Add(hashTag);
            }
        }

        private void SaveSharedPosts(string? ids, int profileId)
        {
            if (ids != null)
            {
                var idList = ids.Split(',');
                List<int> idListConverted = idList.Select(s => int.Parse(s)).ToList();

                foreach (var id in idListConverted)
                {
                    var sharedPost = new SharedPost() { OriginalPostId = id, ProfileId = profileId };
                    sharedPostService.Add(sharedPost);
                }
            }
        }

        private bool AllProfilesExist(string? ids)
        {
            if ( ids == null)
            {
                return true;
            }
            var idList = ids.Split(',');
            List<int> idListConverted = idList.Select(s => int.Parse(s)).ToList();

            var allProfilesValid = idListConverted.All(id => profileService.GetById(id) != null);

            return allProfilesValid;
        }
    }
}
