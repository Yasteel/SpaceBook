namespace Spacebook.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class CreateController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
