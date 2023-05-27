using Spacebook.Interfaces;
using Spacebook.Models;

namespace Spacebook.Services
{
	public class ProfileService : GenericService<Profile>, IProfileService
	{
        public ProfileService(ApplicationDbContext context)
            :base(context)
        {
            
        }

    }
}
