using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Foundation.ApiExtensions.Controllers
{
    public class BaseApiController : ControllerBase  
    {
        protected IActionResult Json<T>(T response) where T : class
        {
            return this.Content(JsonConvert.SerializeObject(response));
        }
    }
}
