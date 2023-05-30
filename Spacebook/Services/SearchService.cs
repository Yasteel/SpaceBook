using Microsoft.EntityFrameworkCore;
using Spacebook.Data;
using Spacebook.Interfaces;
using Spacebook.Models;
using System.ComponentModel;
using System.Security.Policy;

namespace Spacebook.Services
{
    public class SearchService : ISearchService
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthDbContext _authContext;

        public SearchService(ApplicationDbContext context, AuthDbContext authDbContext) 
        {
            this._context = context;
            this._authContext = authDbContext;
        }

        public List<SearchResult> Searching(string searchTerm) 
        {
            return this.post(searchTerm).Concat(this.hashTag(searchTerm)).Concat(this.users(searchTerm)).ToList();
        }

        public List<SearchResult> post(string lowerCaseSearchTerm) 
        {
            var away = _context.Post
                .Where(post => post.Caption.ToLower().Contains(lowerCaseSearchTerm))
                .Select(post => new SearchResult { Type = "Post", Text = post.PostId.ToString() })
                .ToList();
            return _context.Post
                .Where(post => post.Caption.ToLower().Contains(lowerCaseSearchTerm))
                .Select(post => new SearchResult { Type = "Post", Text = post.PostId.ToString() })
                .ToList();
        }

        public List<SearchResult> hashTag(string lowerCaseSearchTerm)
        {
            return _context.HashTags
                .Where(hash => hash.HashTagText.ToLower().Contains(lowerCaseSearchTerm))
                .Select(hash => new SearchResult { Type = "HashTag", Text = hash.HashTagText })
                .ToList();
        }


        public List<SearchResult> users(string lowerCaseSearchTerm) 
        {
           return _authContext.Users
                .Where(user => user.UserName.ToLower().Contains(lowerCaseSearchTerm))
                .Select(user => new SearchResult { Type = "User", Text = user.Email })
                .ToList();
        }


        public class SearchResult
            {
                public string Type { get; set; }
                public string Text { get; set; }
            }

    }
}
