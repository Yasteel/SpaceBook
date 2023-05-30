using Spacebook.Models;
using static Spacebook.Services.SearchService;

namespace Spacebook.Interfaces
{
    public interface ISearchService 
    {
        public List<SearchResult> Searching(string searchTerm);
        public List<SearchResult> post(string lowerCaseSearchTerm);

        public List<SearchResult> hashTag(string lowerCaseSearchTerm);

        public List<SearchResult> users(string lowerCaseSearchTerm);
    }
}
