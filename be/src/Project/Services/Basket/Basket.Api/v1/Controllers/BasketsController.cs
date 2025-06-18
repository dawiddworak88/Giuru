using Basket.Api.v1.RequestModels;
using Basket.Api.Services;
using Basket.Api.ServicesModelsValidators;
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
using Basket.Api.ServicesModels;
using Basket.Api.v1.ResponseModels;
using IdentityModel;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using Basket.Api.Configurations;

namespace Basket.Api.v1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    [ApiController]
    public class BasketsController : BaseApiController
    {
        private readonly IBasketService _basketService;
        private readonly IOptions<AppSettings> _options;

        public BasketsController(
            IBasketService basketService,
            IOptions<AppSettings> options)
        {
            _basketService = basketService;
            _options = options;
        }

        /// <summary>
        /// Updates a basket.
        /// </summary>
        /// <param name="request">The basket details to save.</param>
        /// <returns>The basket.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(BasketResponseModel))]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Post(BasketRequestModel request)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);
            var isSellerClaim = User.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Role && x.Value == AccountConstants.Roles.Seller);

            var serviceModel = new UpdateBasketServiceModel
            {
                Id = request.Id ?? Guid.NewGuid(),
                IsSeller = isSellerClaim is not null,
                Items = request.Items.OrEmptyIfNull().Select(x => new UpdateBasketItemServiceModel 
                { 
                    ProductId = x.ProductId,
                    ProductSku = x.ProductSku,
                    ProductName = x.ProductName,
                    PictureUrl = x.PictureUrl,
                    Quantity = x.Quantity,
                    StockQuantity = x.StockQuantity,
                    OutletQuantity = x.OutletQuantity,
                    UnitPrice = x.UnitPrice,
                    Price = x.Price,
                    Currency = x.Currency,
                    ExternalReference = x.ExternalReference,
                    MoreInfo = x.MoreInfo
                }),
                Language = CultureInfo.CurrentCulture.Name,
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new UpdateBasketModelValidator(_options.Value.MaxAllowedOrderQuantity);
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var basket = await _basketService.UpdateAsync(serviceModel);

                if (basket is not null)
                {
                    var response = new BasketResponseModel
                    {
                        Id = basket.Id,
                        Items = basket.Items.OrEmptyIfNull().Select(x => new BasketItemResponseModel
                        {
                            ProductId = x.ProductId,
                            ProductSku = x.ProductSku,
                            ProductName = x.ProductName,
                            PictureUrl = x.PictureUrl,
                            Quantity = x.Quantity,
                            StockQuantity = x.StockQuantity,
                            OutletQuantity = x.OutletQuantity,
                            UnitPrice = x.UnitPrice,
                            Price = x.Price,
                            Currency = x.Currency,
                            ExternalReference = x.ExternalReference,
                            MoreInfo = x.MoreInfo
                        })
                    };

                    return StatusCode((int)HttpStatusCode.OK, response);
                }
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Deletes a basket by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>OK.</returns>
        [HttpDelete, MapToApiVersion("1.0")]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);
            var serviceModel = new DeleteBasketServiceModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name,
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new DeleteBasketModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);
            if (validationResult.IsValid)
            {
                await _basketService.DeleteAsync(serviceModel);

                return StatusCode((int)HttpStatusCode.OK);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Gets a basket by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The basket.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(BasketResponseModel))]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Get(Guid? id)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);
            var serviceModel = new GetBasketByIdServiceModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name,
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new GetBasketByIdModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var basket = await _basketService.GetBasketById(serviceModel);

                if (basket is not null)
                {
                    var response = new BasketResponseModel
                    {
                        Id = basket.Id,
                        Items = basket.Items.OrEmptyIfNull().Select(x => new BasketItemResponseModel
                        {
                            ProductId = x.ProductId,
                            ProductSku = x.ProductSku,
                            ProductName = x.ProductName,
                            PictureUrl = x.PictureUrl,
                            Quantity = x.Quantity,
                            StockQuantity = x.StockQuantity,
                            OutletQuantity = x.OutletQuantity,
                            UnitPrice = x.UnitPrice,
                            Price = x.Price,
                            Currency = x.Currency,
                            ExternalReference = x.ExternalReference,
                            MoreInfo = x.MoreInfo
                        })
                    };

                    return StatusCode((int)HttpStatusCode.OK, response);
                }
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Checkout a basket order.
        /// </summary>
        /// <param name="request">The model.</param>
        /// <returns>Accpeted.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [Route("checkout")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> BasketCheckoutPost(BasketCheckoutRequestModel request)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);
            var isSellerClaim = User.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Role && x.Value == AccountConstants.Roles.Seller);
            var serviceModel = new CheckoutBasketServiceModel
            {
                BasketId = request.BasketId,
                IsSeller = isSellerClaim is not null,
                ClientId = request.ClientId,
                ClientName = request.ClientName,
                ClientEmail = request.ClientEmail,
                BillingAddressId = request.BillingAddressId,
                BillingCity = request.BillingCity,
                BillingCompany = request.BillingCompany,
                BillingCountryId = request.BillingCountryId,
                BillingFirstName = request.BillingFirstName,
                BillingLastName = request.BillingLastName,
                BillingPhoneNumber = request.BillingPhoneNumber,
                BillingPostCode = request.BillingPostCode,
                BillingRegion = request.BillingRegion,
                BillingStreet = request.BillingStreet,
                ShippingAddressId = request.ShippingAddressId,
                ShippingCity = request.ShippingCity,
                ShippingCompany= request.ShippingCompany,
                ShippingCountryId = request.ShippingCountryId,
                ShippingFirstName = request.ShippingFirstName,
                ShippingLastName = request.ShippingLastName,
                ShippingPhoneNumber = request.ShippingPhoneNumber,
                ShippingPostCode = request.ShippingPostCode,
                ShippingRegion = request.ShippingRegion,
                ShippingStreet = request.ShippingStreet,
                MoreInfo = request.MoreInfo,
                HasCustomOrder = request.HasCustomOrder,
                HasApprovalToSendEmail = request.HasApprovalToSendEmail,
                Attachments = request.Attachments,
                Language = CultureInfo.CurrentCulture.Name,
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new CheckoutBasketServiceModelValidator();

            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                await _basketService.CheckoutAsync(serviceModel);

                return StatusCode((int)HttpStatusCode.Accepted);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }
    }
}
