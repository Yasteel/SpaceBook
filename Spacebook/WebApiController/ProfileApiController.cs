namespace Spacebook.WebApiController
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    using Spacebook.Interfaces;
    using Spacebook.Models;

    using Newtonsoft.Json;

    [Route("api/[controller]")]
    [ApiController]
    public class ProfileApiController : ControllerBase
    {
        private readonly IProfileService _profileService;
        public ProfileApiController(IProfileService profileService) 
        { 
            this._profileService = profileService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Profile>> Get() 
        { 
            var profiles = this._profileService.GetAll();

            return profiles;
        }
    }
}
