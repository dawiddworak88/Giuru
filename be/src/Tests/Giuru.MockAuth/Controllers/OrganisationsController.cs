using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;

namespace Giuru.MockAuth.Controllers
{
    [Route("api/v1/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class OrganisationsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public OrganisationsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Create()
        {
            return StatusCode((int)HttpStatusCode.Created, new
            {
                Id = Guid.Parse(_configuration.GetValue<string>("OrganisationId"))
            });
        }
    }
}
