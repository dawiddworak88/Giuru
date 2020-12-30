using Foundation.Localization.Services;
using Microsoft.AspNetCore.Mvc;

namespace Foundation.ApiExtensions.Controllers
{
    public class BaseApiController : ControllerBase  
    {
        protected readonly ICultureService cultureService;

        public BaseApiController()
        { 
        }

        public BaseApiController(ICultureService cultureService)
        {
            this.cultureService = cultureService;
        }
    }
}
