using Microsoft.AspNetCore.Mvc;

namespace Spacebook.Controllers
{
    public class NotificationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
