using Microsoft.AspNetCore.Mvc;
using Spacebook.Interfaces;

namespace Spacebook.Controllers
{
    public class CreateController : Controller
    {
        private readonly IAzureBlobStorageService storageService;

        public CreateController(IAzureBlobStorageService storageService) 
        { 
            this .storageService = storageService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(IFormFile file) 
        {
            string userId = "test";
            // use this to save to database
            var fileURI = storageService.UploadBlob(file, userId);
            Console.WriteLine(fileURI);

            storageService.GetAllBlobs("test");

            return View(file);
        }
    }
}
