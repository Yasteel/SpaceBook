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
    using Microsoft.IdentityModel.Tokens;

    [Authorize]
    public class CreateApiController: Controller
    {
        private readonly IAzureBlobStorageService storageService;
        private readonly IPostService postService;
        private readonly UserManager<SpacebookUser> userManager;
        private readonly IHashTagService hashTagService;
        private readonly IValidator<Post> validator;
        private readonly IProfileService profileService;
        private readonly ISharedPostService sharedPostService;

        public CreateApiController(IAzureBlobStorageService storageService,
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

        public IActionResult Post(Post post)
        {
            if (post == null)
            {
                return this.BadRequest("{\"Error\":[\"Could not complete request. Invalid data.\"]}");
            }

            ValidationResult result = this.validator.Validate(post);

            if (!result.IsValid)
            {
                return this.BadRequest(result.Errors);
            }

            if (!this.AllProfilesExist(post.SharedIDs))
            {
                return this.BadRequest("{\"Error\":[\"Could not complete request. Invalid data.\"]}");
            }

            

            // use this to save to database
            string? fileURI;
            string userId = userManager.GetUserId(User);
            if (post.ImageFile != null) 
            {
                fileURI = storageService.UploadBlob(post.ImageFile, userId);
                post.MediaUrl = fileURI;
                post.Type = post.ImageFile.ContentType;
            }
            else if (post.VideoFile != null)
            {
                fileURI = storageService.UploadBlob(post.VideoFile, userId);
                post.MediaUrl = fileURI;
                post.Type = post.VideoFile.ContentType;
            }

            post.Timestamp = DateTime.Now;

            //Get profile id
            var userEmail = userManager.GetUserName(User);
            var profile = profileService.GetByEmail(userEmail);

            post.ProfileId = (int) profile.UserId;

            this.postService.Add(post);

            SaveHashTags(post.Caption, post.PostId);
            SaveSharedPosts(post.SharedIDs, post.ProfileId);

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
                    HashTagText = tag.Substring(1).ToLower(),
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
            if (ids.IsNullOrEmpty())
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
