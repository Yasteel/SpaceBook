namespace Spacebook.Services
{
    using Spacebook.Interfaces;
    using Spacebook.Models;
    public class LikesService: GenericService<Likes>, ILikesService
    {
        public LikesService(ApplicationDbContext context) 
            :base(context) { }
    }
}
