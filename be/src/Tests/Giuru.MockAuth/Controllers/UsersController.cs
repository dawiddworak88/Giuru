using Giuru.MockAuth.Definitions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Giuru.MockAuth.Controllers
{
    [Route("api/v1/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public UsersController() { }

        [HttpGet]
        [Route("{email}")]
        public IActionResult GetByEmail(string email)
        {
            return StatusCode((int)HttpStatusCode.OK, new
            {
                Id = AuthConstants.UserId,
                Email = email
            });
        }
    }
}
