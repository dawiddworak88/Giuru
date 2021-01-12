using Basket.Api.v1.Areas.Baskets.Models;
using Basket.Api.v1.Areas.Baskets.RequestModels;
using Basket.Api.v1.Areas.Baskets.Services;
using Basket.Api.v1.Areas.Baskets.Validators;
using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Controllers;
using Foundation.Extensions.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Basket.Api.v1.Areas.Baskets.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [Authorize]
    [ApiController]
    public class BasketsController : BaseApiController
    {
        private readonly IBasketService basketService;

        public BasketsController(IBasketService basketService)
        {
            this.basketService = basketService;
        }

        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Post(BasketRequestModel request)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.OrganisationIdClaim);

            var serviceModel = new UpdateBasketServiceModel
            {
                Id = request.Id ?? Guid.NewGuid(),
                Items = request.Items.OrEmptyIfNull().Select(x => new BasketItemServiceModel 
                { 
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    DeliveryFrom = x.DeliveryFrom,
                    DeliveryTo = x.DeliveryTo,
                    MoreInfo = x.MoreInfo
                }),
                Language = CultureInfo.CurrentCulture.Name,
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new UpdateBasketModelValidator();

            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var basket = await this.basketService.UpdateAsync(serviceModel);

                return this.StatusCode((int)HttpStatusCode.OK, basket);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }
    }
}
