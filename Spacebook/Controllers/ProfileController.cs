using Microsoft.AspNetCore.Mvc;

namespace Spacebook.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
