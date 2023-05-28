using Spacebook.Interfaces;
using Spacebook.Models;

namespace Spacebook.Services
{
	public class ProfileService : GenericService<Profile>, IProfileService
	{
		private readonly ApplicationDbContext context;

		public ProfileService(ApplicationDbContext context)
            : base(context)
        {
			this.context = context;
		}

        public Profile GetByUsername(string username)
        {
            return this.context.Profile.FirstOrDefault(_ => _.Username == username)!;
        }
    }
}
