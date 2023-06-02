using Microsoft.AspNetCore.Identity;
using Spacebook.Interfaces;
using Spacebook.Models;

namespace Spacebook.Services
{
    public class NotificationService : GenericService<Notification>, INotificationService
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<SpacebookUser> userManager;

        public NotificationService(ApplicationDbContext context, UserManager<SpacebookUser> userManager)
            : base(context)
        {
            this.context = context;
            this.userManager = userManager;
        }

        //public async Task<List<Notification>> GetNotificationAsync(string email) 
        //{
        //   //var spacebookUser = (SpacebookUser)await this.userManager.GetUserAsync(User);
        //   // var thisUserProfile = spacebookUser.Email;

        //    return context.Notification
        //        .Where(n => n.u)
        //}

        public class NotificationResult 
        { 
            public string NoifyText { get; set; }
        }
    }
}
