namespace Spacebook.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Spacebook.Interfaces;
    using Spacebook.Models;
    using Spacebook.RecommendationEngine.Interfaces;
    using Spacebook.Services;
    public class ForYouController : Controller
    {
        private readonly IRecommdationService recommenderService;
        private readonly IProfileService profileService;
        private readonly UserManager<SpacebookUser> userManager;


        public ForYouController(IRecommdationService recommdationService,
                                UserManager<SpacebookUser> userManager,
                                IProfileService profileService) 
        { 
            this.recommenderService = recommdationService;
            this.userManager = userManager;
            this.profileService = profileService;
        }

        public IActionResult Index()
        {
            //Get profile id
            var userEmail = userManager.GetUserName(User);
            var profile = profileService.GetByEmail(userEmail);

            recommenderService.GetPosts((int) profile.UserId);
            return View();
        }
    }
}
