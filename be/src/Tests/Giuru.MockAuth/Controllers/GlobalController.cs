using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace Giuru.MockAuth.Controllers
{
    /// <summary>
    /// Stands in for the Global API, which the integration run does not start. The web apps only read the countries and
    /// currencies to describe a client, and an empty list is a valid answer.
    /// </summary>
    [Route("api/v1")]
    [AllowAnonymous]
    [ApiController]
    public class GlobalController : ControllerBase
    {
        [HttpGet("countries")]
        public IActionResult GetCountries()
        {
            return StatusCode((int)HttpStatusCode.OK, new { Data = Array.Empty<object>(), Total = 0, PageSize = 50 });
        }

        [HttpGet("currencies")]
        public IActionResult GetCurrencies()
        {
            return StatusCode((int)HttpStatusCode.OK, new { Data = Array.Empty<object>(), Total = 0, PageSize = 50 });
        }
    }
}
