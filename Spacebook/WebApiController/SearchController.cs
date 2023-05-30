
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spacebook;
using Spacebook.Data;
using Spacebook.Interfaces;
using System.Linq;

namespace Spacebook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet]
        public IActionResult Search(string searchTerm)
        {
            var lowerCaseSearchTerm = searchTerm.ToLower();

            return Ok(_searchService.Searching(lowerCaseSearchTerm));
        }
    }
}

