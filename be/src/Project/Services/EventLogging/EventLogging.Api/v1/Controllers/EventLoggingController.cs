using EventLogging.Api.Services;
using EventLogging.Api.v1.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace EventLogging.Api.v1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [AllowAnonymous]
    [ApiController]
    public class EventLoggingController : ControllerBase
    {
        private readonly IEventLoggingService eventLoggingService;

        public EventLoggingController(IEventLoggingService eventLoggingService)
        {
            this.eventLoggingService = eventLoggingService;
        }

        /// <summary>
        /// Creates an event log entry.
        /// </summary>
        /// <param name="request">The model.</param>
        /// <returns>OK.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Save(EventLogRequestModel request)
        {
            await this.eventLoggingService.LogAsync(
                    request.EventId,
                    request.EventState,
                    request.Content,
                    request.EntityId,
                    request.EntityType,
                    request.OldValue,
                    request.NewValue,
                    request.OrganisationId,
                    request.Username,
                    request.Source,
                    request.IpAddress
                );

            return this.StatusCode((int)HttpStatusCode.OK);
        }
    }
}
