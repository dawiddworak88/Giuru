using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Controllers;
using Foundation.Extensions.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.Helpers;
using Foundation.GenericRepository.Paginations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Ordering.Api.Services;
using Ordering.Api.ServicesModels;
using Ordering.Api.Validators;
using Ordering.Api.v1.ResponseModels;
using Ordering.Api.v1.RequestModels;

namespace Ordering.Api.v1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    [ApiController]
    public class OrdersController : BaseApiController
    {
        private readonly IOrdersService ordersService;

        public OrdersController(IOrdersService ordersService)
        {
            this.ordersService = ordersService;
        }

        /// <summary>
        /// Gets list of orders.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <param name="orderBy">The optional order by.</param>
        /// <param name="createdDateGreaterThan">The optional iso date and time to get orders where created date is greater than.</param>
        /// <returns>The list of orders.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Get(string searchTerm, int pageIndex, int itemsPerPage, string orderBy, DateTime? createdDateGreaterThan)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetOrdersServiceModel
            {
                Language = CultureInfo.CurrentCulture.Name,
                SearchTerm = searchTerm,
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage,
                OrderBy = orderBy,
                CreatedDateGreaterThan = createdDateGreaterThan,
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                IsSeller = this.User.IsInRole("Seller")
            };

            var validator = new GetOrdersModelValidator();

            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var orders = await this.ordersService.GetAsync(serviceModel);

                if (orders != null)
                {
                    var response = new PagedResults<IEnumerable<OrderResponseModel>>(orders.Total, orders.PageSize)
                    {
                        Data = orders.Data.OrEmptyIfNull().Select(x => new OrderResponseModel
                        {
                            Id = x.Id,
                            ClientId = x.ClientId,
                            ClientName = x.ClientName,
                            BillingAddressId = x.BillingAddressId,
                            BillingCity = x.BillingCity,
                            BillingCompany = x.BillingCompany,
                            BillingCountryCode = x.BillingCountryCode,
                            BillingFirstName = x.BillingFirstName,
                            BillingLastName = x.BillingLastName,
                            BillingPhone = x.BillingPhone,
                            BillingPhonePrefix = x.BillingPhonePrefix,
                            BillingPostCode = x.BillingPostCode,
                            BillingRegion = x.BillingRegion,
                            BillingStreet = x.BillingStreet,
                            ShippingAddressId = x.ShippingAddressId,
                            ShippingCity = x.ShippingCity,
                            ShippingCompany = x.ShippingCompany,
                            ShippingCountryCode = x.ShippingCountryCode,
                            ShippingFirstName = x.ShippingFirstName,
                            ShippingLastName = x.ShippingLastName,
                            ShippingPhone = x.ShippingPhone,
                            ShippingPhonePrefix = x.ShippingPhonePrefix,
                            ShippingPostCode = x.ShippingPostCode,
                            ShippingRegion = x.ShippingRegion,
                            ShippingStreet = x.ShippingStreet,
                            ExpectedDeliveryDate = x.ExpectedDeliveryDate,
                            MoreInfo = x.MoreInfo,
                            Reason = x.Reason,
                            OrderStateId = x.OrderStateId,
                            OrderStatusId = x.OrderStatusId,
                            OrderStatusName = x.OrderStatusName,
                            OrderItems = x.OrderItems.Select(y => new OrderItemResponseModel
                            {
                                ProductId = y.ProductId,
                                ProductSku = y.ProductSku,
                                ProductName = y.ProductName,
                                PictureUrl = y.PictureUrl,
                                Quantity = y.Quantity,
                                StockQuantity = y.StockQuantity,
                                OutletQuantity = y.OutletQuantity,
                                ExternalReference = y.ExternalReference,
                                ExpectedDeliveryFrom = y.ExpectedDeliveryFrom,
                                ExpectedDeliveryTo = y.ExpectedDeliveryTo,
                                MoreInfo = y.MoreInfo,
                                LastModifiedDate = y.LastModifiedDate,
                                CreatedDate = y.CreatedDate
                            }),
                            LastModifiedDate = x.LastModifiedDate,
                            CreatedDate = x.CreatedDate
                        })
                    };

                    return this.StatusCode((int)HttpStatusCode.OK, response);
                }
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Gets order by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The order.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Get(Guid? id)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetOrderServiceModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                IsSeller = this.User.IsInRole("Seller")
            };

            var validator = new GetOrderModelValidator();

            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var order = await this.ordersService.GetAsync(serviceModel);

                if (order != null)
                {
                    var response = new OrderResponseModel
                    {
                        Id = order.Id,
                        ClientId = order.ClientId,
                        ClientName = order.ClientName,
                        BillingAddressId = order.BillingAddressId,
                        BillingCity = order.BillingCity,
                        BillingCompany = order.BillingCompany,
                        BillingCountryCode = order.BillingCountryCode,
                        BillingFirstName = order.BillingFirstName,
                        BillingLastName = order.BillingLastName,
                        BillingPhone = order.BillingPhone,
                        BillingPhonePrefix = order.BillingPhonePrefix,
                        BillingPostCode = order.BillingPostCode,
                        BillingRegion = order.BillingRegion,
                        BillingStreet = order.BillingStreet,
                        ShippingAddressId = order.ShippingAddressId,
                        ShippingCity = order.ShippingCity,
                        ShippingCompany = order.ShippingCompany,
                        ShippingCountryCode = order.ShippingCountryCode,
                        ShippingFirstName = order.ShippingFirstName,
                        ShippingLastName = order.ShippingLastName,
                        ShippingPhone = order.ShippingPhone,
                        ShippingPhonePrefix = order.ShippingPhonePrefix,
                        ShippingPostCode = order.ShippingPostCode,
                        ShippingRegion = order.ShippingRegion,
                        ShippingStreet = order.ShippingStreet,
                        ExpectedDeliveryDate = order.ExpectedDeliveryDate,
                        MoreInfo = order.MoreInfo,
                        Reason = order.Reason,
                        OrderStateId = order.OrderStateId,
                        OrderStatusId = order.OrderStatusId,
                        OrderStatusName = order.OrderStatusName,
                        OrderItems = order.OrderItems.Select(x => new OrderItemResponseModel
                        {
                            ProductId = x.ProductId,
                            ProductSku = x.ProductSku,
                            ProductName = x.ProductName,
                            PictureUrl = x.PictureUrl,
                            Quantity = x.Quantity,
                            StockQuantity = x.StockQuantity,
                            OutletQuantity = x.OutletQuantity,
                            ExternalReference = x.ExternalReference,
                            ExpectedDeliveryFrom = x.ExpectedDeliveryFrom,
                            ExpectedDeliveryTo = x.ExpectedDeliveryTo,
                            MoreInfo = x.MoreInfo,
                            LastModifiedDate = x.LastModifiedDate,
                            CreatedDate = x.CreatedDate
                        }),
                        LastModifiedDate = order.LastModifiedDate,
                        CreatedDate = order.CreatedDate
                    };

                    return this.StatusCode((int)HttpStatusCode.OK, response);
                }
                else
                {
                    return this.StatusCode((int)HttpStatusCode.NotFound);
                }

            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        ///  Updates the order status.
        /// </summary>
        /// <returns>The updated order status.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [Route("orderstatus")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Post(UpdateOrderStatusRequestModel model)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new UpdateOrderStatusServiceModel
            {
                OrderId = model.OrderId,
                OrderStatusId = model.OrderStatusId,
                Language = CultureInfo.CurrentCulture.Name,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                IsSeller = this.User.IsInRole("Seller")
            };

            var validator = new UpdateOrderStatusModelValidator();

            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var order = await this.ordersService.SaveOrderStatusAsync(serviceModel);

                if (order != null)
                {
                    var response = new OrderResponseModel
                    {
                        Id = order.Id,
                        ClientId = order.ClientId,
                        ClientName = order.ClientName,
                        BillingAddressId = order.BillingAddressId,
                        BillingCity = order.BillingCity,
                        BillingCompany = order.BillingCompany,
                        BillingCountryCode = order.BillingCountryCode,
                        BillingFirstName = order.BillingFirstName,
                        BillingLastName = order.BillingLastName,
                        BillingPhone = order.BillingPhone,
                        BillingPhonePrefix = order.BillingPhonePrefix,
                        BillingPostCode = order.BillingPostCode,
                        BillingRegion = order.BillingRegion,
                        BillingStreet = order.BillingStreet,
                        ShippingAddressId = order.ShippingAddressId,
                        ShippingCity = order.ShippingCity,
                        ShippingCompany = order.ShippingCompany,
                        ShippingCountryCode = order.ShippingCountryCode,
                        ShippingFirstName = order.ShippingFirstName,
                        ShippingLastName = order.ShippingLastName,
                        ShippingPhone = order.ShippingPhone,
                        ShippingPhonePrefix = order.ShippingPhonePrefix,
                        ShippingPostCode = order.ShippingPostCode,
                        ShippingRegion = order.ShippingRegion,
                        ShippingStreet = order.ShippingStreet,
                        ExpectedDeliveryDate = order.ExpectedDeliveryDate,
                        MoreInfo = order.MoreInfo,
                        Reason = order.Reason,
                        OrderStateId = order.OrderStateId,
                        OrderStatusId = order.OrderStatusId,
                        OrderStatusName = order.OrderStatusName,
                        OrderItems = order.OrderItems.Select(x => new OrderItemResponseModel
                        {
                            ProductId = x.ProductId,
                            ProductSku = x.ProductSku,
                            ProductName = x.ProductName,
                            PictureUrl = x.PictureUrl,
                            Quantity = x.Quantity,
                            StockQuantity = x.StockQuantity,
                            OutletQuantity = x.OutletQuantity,
                            ExternalReference = x.ExternalReference,
                            ExpectedDeliveryFrom = x.ExpectedDeliveryFrom,
                            ExpectedDeliveryTo = x.ExpectedDeliveryTo,
                            MoreInfo = x.MoreInfo,
                            LastModifiedDate = x.LastModifiedDate,
                            CreatedDate = x.CreatedDate
                        }),
                        LastModifiedDate = order.LastModifiedDate,
                        CreatedDate = order.CreatedDate
                    };

                    return this.StatusCode((int)HttpStatusCode.OK, response);
                }
                else
                {
                    return this.StatusCode((int)HttpStatusCode.NotFound);
                }

            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }
    }
}