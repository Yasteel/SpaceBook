using Microsoft.AspNetCore.Mvc;
using Spacebook.Interfaces;

namespace Spacebook.Controllers
{
    public class SearchFunctionalityController : Controller
    {
        private readonly ISearchFunctionalityService searchFunctionalityService;

        public SearchFunctionalityController(ISearchFunctionalityService service)
        {
            this.searchFunctionalityService = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Search(string? searchTerm)
        {
            searchTerm = searchTerm?.ToLower();

            var results = searchFunctionalityService.posts(searchTerm);

            return Json(results);
        }
    }
}
