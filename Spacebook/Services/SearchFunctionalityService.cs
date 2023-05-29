using Microsoft.EntityFrameworkCore;
using Spacebook.Interfaces;
using Spacebook.Models;

namespace Spacebook.Services
{
    public class SearchFunctionalityService : ISearchFunctionalityService
    {
        private readonly ApplicationDbContext context;

        public SearchFunctionalityService(ApplicationDbContext _context) 
        {
            this.context = _context;
        }

        public List<Post> posts(string searchItem)
        {
            return this.context.Post.Where(search =>
                search.Type.ToLower().Contains(searchItem)).ToList();
        }
    }
}
