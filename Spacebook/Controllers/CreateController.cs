namespace Spacebook.Controllers
{
    using System.Linq;
    using System.Text.RegularExpressions;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using Spacebook.Data;
    using Spacebook.Interfaces;
    using Spacebook.Models;
   
    [Authorize]
    public class CreateController : Controller
    {
        private readonly IAzureBlobStorageService storageService;
        private readonly IPostService postService;
        private readonly UserManager<SpacebookUser> userManager;
        private readonly IHashTagService hashTagService;

        public CreateController(IAzureBlobStorageService storageService,
                                IPostService postService,
                                UserManager<SpacebookUser> userManager,
                                IHashTagService hashTagService) 
        { 
            this .storageService = storageService;
            this.postService = postService;
            this.userManager = userManager;
            this.hashTagService = hashTagService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Post model) 
        {
            string userId = userManager.GetUserId(User);

            // use this to save to database
            var fileURI = storageService.UploadBlob(model.Media, userId);

            //TODO use TagService to store tags
            model.MediaUrl = fileURI;
            //Is profile id the same as authenticated user id?
            model.Type = model.Media.ContentType;
            model.Timestamp = DateTime.Now;
            model.ProfileId = 1;
            

            this.postService.Add(model);

            SaveHashTags(model.Caption, model.PostId);

            Console.WriteLine("Post added successfully");

            return RedirectToAction("Index", "Home");
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
    }
}
