using Spacebook.Interfaces;
using Spacebook.Models;

namespace Spacebook.Services
{
    using Spacebook.Interfaces;
    using Spacebook.Models;
    public class PostService: GenericService<Post>, IPostService
    {
        public PostService(ApplicationDbContext context)
            :base(context)
        { }
    }
}
