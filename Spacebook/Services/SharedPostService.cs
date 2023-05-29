namespace Spacebook.Services
{
    using Spacebook.Interfaces;
    using Spacebook.Models;
    public class SharedPostService: GenericService<SharedPost>, ISharedPostService
    {
        public SharedPostService(ApplicationDbContext context) : base(context) { }
    }
}
