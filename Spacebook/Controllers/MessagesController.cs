using Microsoft.AspNetCore.Mvc;

namespace Spacebook.Controllers
{
    public class MessagesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
