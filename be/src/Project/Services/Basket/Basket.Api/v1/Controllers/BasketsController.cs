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
using Newtonsoft.Json;

namespace Basket.Api.v1.Controllers
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(BasketResponseModel))]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Post(BasketRequestModel request)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.OrganisationIdClaim);
            var isSellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.IsSellerClaim)?.Value;

            var serviceModel = new UpdateBasketServiceModel
            {
                Id = request.Id ?? Guid.NewGuid(),
                IsSeller = bool.Parse(isSellerClaim),
                Items = request.Items.OrEmptyIfNull().Select(x => new UpdateBasketItemServiceModel 
                { 
                    ProductId = x.ProductId,
                    ProductSku = x.ProductSku,
                    ProductName = x.ProductName,
                    PictureUrl = x.PictureUrl,
                    Quantity = x.Quantity,
                    ExternalReference = x.ExternalReference,
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

                if (basket != null)
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
                            ExternalReference = x.ExternalReference,
                            DeliveryFrom = x.DeliveryFrom,
                            DeliveryTo = x.DeliveryTo,
                            MoreInfo = x.MoreInfo
                        })
                    };

                    return this.StatusCode((int)HttpStatusCode.OK, response);
                }
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        [HttpDelete, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Delete()
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.OrganisationIdClaim);
            var serviceModel = new DeleteBasketServiceModel
            {
                Language = CultureInfo.CurrentCulture.Name,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };
            var validator = new DeleteBasketModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);
            if (validationResult.IsValid)
            {
                await this.basketService.DeleteAsync(serviceModel);

                this.StatusCode((int)HttpStatusCode.OK);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        [HttpGet, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(BasketResponseModel))]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Get()
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.OrganisationIdClaim);
            var serviceModel = new GetBasketByOrganisationServiceModel
            {
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new GetBasketByOrganisationModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);
            if (validationResult.IsValid)
            {
                var basket = await this.basketService.GetByOrganisation(serviceModel);
                if (basket != null)
                {
                    var response = new BasketOrderResponseModel
                    {
                        Id = basket.Id,
                        OwnerId = basket.OwnerId,
                        Items = basket.Items.OrEmptyIfNull().Select(x => new BasketOrderItemResponseModel
                        {
                            ProductId = x.ProductId,
                            Sku = x.Sku,
                            Name = x.Name,
                            ImageSrc = x.ImageSrc,
                            ImageAlt = x.ImageAlt,
                            Quantity = x.Quantity,
                            ExternalReference = x.ExternalReference,
                            DeliveryFrom = x.DeliveryFrom,
                            DeliveryTo = x.DeliveryTo,
                            MoreInfo = x.MoreInfo
                        })
                    };

                    return this.StatusCode((int)HttpStatusCode.OK, response);
                }
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        [HttpPost, MapToApiVersion("1.0")]
        [Route("checkout")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> BasketCheckoutPost(BasketCheckoutRequestModel request)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.OrganisationIdClaim);

            var serviceModel = new CheckoutBasketServiceModel
            {
                BasketId = request.BasketId,
                ClientId = request.ClientId,
                ClientName = request.ClientName,
                BillingAddressId = request.BillingAddressId,
                BillingCity = request.BillingCity,
                BillingCompany = request.BillingCompany,
                BillingCountryCode = request.BillingCountryCode,
                BillingFirstName = request.BillingFirstName,
                BillingLastName = request.BillingLastName,
                BillingPhone = request.BillingPhone,
                BillingPhonePrefix = request.BillingPhonePrefix,
                BillingPostCode = request.BillingPostCode,
                BillingRegion = request.BillingRegion,
                BillingStreet = request.BillingStreet,
                ShippingAddressId = request.ShippingAddressId,
                ShippingCity = request.ShippingCity,
                ShippingCompany= request.ShippingCompany,
                ShippingCountryCode = request.ShippingCountryCode,
                ShippingFirstName = request.ShippingFirstName,
                ShippingLastName = request.ShippingLastName,
                ShippingPhone = request.ShippingPhone,
                ShippingPhonePrefix = request.ShippingPhonePrefix,
                ShippingPostCode = request.ShippingPostCode,
                ShippingRegion = request.ShippingRegion,
                ShippingStreet = request.ShippingStreet,
                ExpectedDeliveryDate = request.ExpectedDeliveryDate,
                MoreInfo = request.MoreInfo,
                Language = CultureInfo.CurrentCulture.Name,
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new CheckoutBasketServiceModelValidator();

            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                await this.basketService.CheckoutAsync(serviceModel);

                return this.StatusCode((int)HttpStatusCode.Accepted);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }
    }
}
