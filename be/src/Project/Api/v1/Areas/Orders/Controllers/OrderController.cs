using Api.v1.Areas.Orders.RequestModels;
using Api.v1.Areas.Orders.ResponseModels;
using Feature.Account.Definitions;
using Feature.Order.Models;
using Feature.Order.Services;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Helpers;
using Foundation.Extensions.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Api.v1.Areas.Schemas.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [Authorize]
    [ApiController]
    public class OrderController : BaseApiController
    {
        private readonly IOrderService orderService;

        private readonly ILogger logger;

        public OrderController(IOrderService orderService, ILogger<SchemaController> logger)
        {
            this.orderService = orderService;
            this.logger = logger;
        }

        /// <summary>
        /// Validates whether the order can be placed.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Returns whether the validation of the order has been successful.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [Route("Validation")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Validate([FromBody] ValidateOrderRequestModel model)
        {
            try
            {
                var tenantClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.TenantIdClaim);

                var validateOrderModel = new OrderValidationModel
                {
                    ClientId = model.ClientId,
                    OrderItems = model.OrderItems != null ? model.OrderItems.Select(x => new OrderItemModel { Sku = x.Sku, Quantity = x.Quantity, SchemaId = x.SchemaId, FormData = x.FormData }) : null,
                    Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    TenantId = GuidHelper.ParseNullable(tenantClaim?.Value),
                    Language = model.Language
                };

                var resultModel = await this.orderService.ValidateOrderAsync(validateOrderModel);

                return this.StatusCode((int)HttpStatusCode.OK, new OrderValidationResponseModel(resultModel.Errors));
            }
            catch (Exception exception)
            {
                var error = ErrorHelper.GenerateErrorSignature(Assembly.GetExecutingAssembly().ToString());
                this.logger.LogError(exception, $"{error.ErrorId} - {error.ErrorSource}");
                return this.StatusCode((int)HttpStatusCode.BadRequest, new OrderValidationResponseModel { Error = error });
            }
        }
    }
}
