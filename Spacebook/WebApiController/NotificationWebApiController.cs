using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Spacebook.Interfaces;

namespace Spacebook.WebApiController
{
    public class NotificationWebApiController : Controller
    {
        private readonly INotificationService notificationService;

        public NotificationWebApiController(INotificationService notificationService)
        {
            this.notificationService = notificationService;
        }

        [HttpGet]
        public object Get(DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(this.notificationService.GetAll(), loadOptions);
            //return JsonConvert.SerializeObject(this.notificationService.GetAll());
        }
    }
}
