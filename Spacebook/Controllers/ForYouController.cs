namespace Spacebook.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Spacebook.Interfaces;
    using Spacebook.Models;
    using Spacebook.RecommendationEngine.Interfaces;

    public class ForYouController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
