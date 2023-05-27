namespace Spacebook.Services
{
    using Spacebook.Interfaces;
    using Spacebook.Models;

    public class ProfileService: GenericService<Profile>, IProfileService
    {
        public ProfileService(ApplicationDbContext context) 
        :base(context)
        { }
    }
}
