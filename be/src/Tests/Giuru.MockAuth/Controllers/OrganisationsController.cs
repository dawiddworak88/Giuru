using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace Giuru.MockAuth.Controllers
{
    [Route("api/v1/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class OrganisationsController : ControllerBase
    {
        public OrganisationsController(){}

        [HttpPost]
        public IActionResult Create()
        {
            return StatusCode((int)HttpStatusCode.Created, new
            {
                Id = Guid.Parse("09affcc9-1665-45d6-919f-3d2026106ba1")
            });
        }
    }
}
