using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Controllers;
using Foundation.Extensions.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Ordering.Api.ServicesModels;
using Ordering.Api.v1.ResponseModels;
using Ordering.Api.Services.Orders;

namespace Ordering.Api.v1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    [ApiController]
    public class OrderStatusesController : BaseApiController
    {
        private readonly IOrdersService _ordersService;

        public OrderStatusesController(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        /// <summary>
        /// Gets list of order statuses.
        /// </summary>
        /// <returns>The list of order statuses.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Get()
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetOrderStatusesServiceModel
            {
                Language = CultureInfo.CurrentCulture.Name,
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var orders = await _ordersService.GetOrderStatusesAsync(serviceModel);

            if (orders is not null)
            {
                var response = orders.Select(x => new OrderStatusResponseModel 
                { 
                    Id = x.Id,
                    Name = x.Name,
                    LastModifiedDate = x.LastModifiedDate,
                    CreatedDate = x.CreatedDate
                });

                return StatusCode((int)HttpStatusCode.OK, response);
            }

            return StatusCode((int)HttpStatusCode.UnprocessableEntity);
        }
    }
}