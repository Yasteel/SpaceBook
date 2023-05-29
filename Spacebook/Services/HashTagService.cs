namespace Spacebook.Services
{
    using Spacebook.Interfaces;
    using Spacebook.Models;
    public class HashTagService: GenericService<HashTag>, IHashTagService
    {
        public HashTagService(ApplicationDbContext context)
            :base(context) 
        { }
    }
}
