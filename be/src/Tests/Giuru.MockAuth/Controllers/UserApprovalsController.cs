using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace Giuru.MockAuth.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserApprovalsController : ControllerBase
    {
        public UserApprovalsController() { }

        [HttpGet]
        [Route("{userId}")]
        public IActionResult GetByUserId()
        {
            return StatusCode((int)HttpStatusCode.OK, Array.Empty<object>());
        }
    }
}