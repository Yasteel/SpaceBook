namespace Spacebook.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class HomeController : Controller
    {

        [Authorize]
        public IActionResult Index()
        {
            
            return View();
        }
    }
}