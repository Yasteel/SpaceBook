namespace Spacebook.Services
{
    using Microsoft.EntityFrameworkCore;
    using Spacebook.Interfaces;
    using Spacebook.Models;

	public class ProfileService : GenericService<Profile>, IProfileService
	{
		private readonly ApplicationDbContext context;

		public ProfileService(ApplicationDbContext context)
            :base(context)
        {
			this.context = context;
		}

        public Profile GetByUsername(string username)
        {
            return this.context.Profile.FirstOrDefault(_ => _.Username == username)!;
        }

        public Profile GetByEmail(string email)
        {
            return this.context.Profile.FirstOrDefault(_ => _.Email == email)!;
        }
    }
}
