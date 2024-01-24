using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Client.Api.v1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}[controller]")]
    [ApiController]
    [Authorize]
    public class ClientNotificationsController : ControllerBase
    {
    }
}
