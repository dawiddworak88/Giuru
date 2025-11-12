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
        private readonly IOrdersService _ordersService;

        public OrdersController(
            IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        /// <summary>
        /// Get list of order files.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <param name="orderBy">The optional order by.</param>
        /// <param name="id">The order id.</param>
        /// <returns>The list of order files.</returns>
        [HttpGet("files/{id}"), MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PagedResults<IEnumerable<OrderFileResponseModel>>))]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Files(Guid? id, string searchTerm, int? pageIndex, int? itemsPerPage, string orderBy)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetOrderFilesServiceModel
            {
                Id = id,
                SearchTerm = searchTerm,
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage,
                OrderBy = orderBy,
                Language = CultureInfo.CurrentCulture.Name,
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new GetOrderFilesModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var orderFiles = await _ordersService.GetOrderFilesAsync(serviceModel);

                if (orderFiles is not null)
                {
                    var response = new PagedResults<IEnumerable<OrderFileResponseModel>>(orderFiles.Total, orderFiles.PageSize)
                    {
                        Data = orderFiles.Data.OrEmptyIfNull().Select(x => new OrderFileResponseModel
                        {
                            Id = x.Id,
                            LastModifiedDate = x.LastModifiedDate,
                            CreatedDate = x.CreatedDate
                        })
                    };

                    return StatusCode((int)HttpStatusCode.OK, response);
                }
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Gets list of orders.
        /// </summary>
        /// <param name="ids">The orders ids.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <param name="orderBy">The optional order by.</param>
        /// <param name="createdDateGreaterThan">The optional iso date and time to get orders where created date is greater than.</param>
        /// <param name="orderStatusId">The optional order status Id to filter orders by specific status.</param>
        /// <returns>The list of orders.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Get(
            string ids, 
            string searchTerm, 
            int? pageIndex, 
            int? itemsPerPage, 
            string orderBy, 
            DateTime? createdDateGreaterThan,
            Guid? orderStatusId)
        {
            var ordersIds = ids.ToEnumerableGuidIds();
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            if (ordersIds is not null)
            {
                var serviceModel = new GetOrdersByIdsServiceModel
                {
                    Ids = ordersIds,
                    SearchTerm = searchTerm,
                    PageIndex = pageIndex,
                    ItemsPerPage = itemsPerPage,
                    OrderBy = orderBy,
                    CreatedDateGreaterThan = createdDateGreaterThan,
                    OrderStatusId = orderStatusId,
                    Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                    Language = CultureInfo.CurrentCulture.Name,
                    IsSeller = User.IsInRole("Seller")
                };

                var validator = new GetOrdersByIdsModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var orders = await _ordersService.GetAsync(serviceModel);

                    if (orders is not null)
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
                                BillingCountryId = x.BillingCountryId,
                                BillingFirstName = x.BillingFirstName,
                                BillingLastName = x.BillingLastName,
                                BillingPhoneNumber = x.BillingPhoneNumber,
                                BillingPostCode = x.BillingPostCode,
                                BillingRegion = x.BillingRegion,
                                BillingStreet = x.BillingStreet,
                                ShippingAddressId = x.ShippingAddressId,
                                ShippingCity = x.ShippingCity,
                                ShippingCompany = x.ShippingCompany,
                                ShippingPhoneNumber = x.ShippingPhoneNumber,
                                ShippingCountryId = x.ShippingCountryId,
                                ShippingFirstName = x.ShippingFirstName,
                                ShippingLastName = x.ShippingLastName,
                                ShippingPostCode = x.ShippingPostCode,
                                ShippingRegion = x.ShippingRegion,
                                ShippingStreet = x.ShippingStreet,
                                MoreInfo = x.MoreInfo,
                                Reason = x.Reason,
                                OrderStateId = x.OrderStateId,
                                OrderStatusId = x.OrderStatusId,
                                OrderStatusName = x.OrderStatusName,
                                OrderItems = x.OrderItems.Select(y => new OrderItemResponseModel
                                {
                                    Id = y.Id,
                                    ProductId = y.ProductId,
                                    ProductSku = y.ProductSku,
                                    ProductName = y.ProductName,
                                    PictureUrl = y.PictureUrl,
                                    Quantity = y.Quantity,
                                    StockQuantity = y.StockQuantity,
                                    OutletQuantity = y.OutletQuantity,
                                    UnitPrice = y.UnitPrice,
                                    Price = y.Price,
                                    Currency = y.Currency,
                                    ExternalReference = y.ExternalReference,
                                    MoreInfo = y.MoreInfo,
                                    OrderItemStateId = y.OrderItemStateId,
                                    OrderItemStatusId = y.OrderItemStatusId,
                                    OrderItemStatusName = y.OrderItemStatusName,
                                    OrderItemStatusChangeComment = y.OrderItemStatusChangeComment,
                                    LastModifiedDate = y.LastModifiedDate,
                                    CreatedDate = y.CreatedDate
                                }),
                                Attachments = x.Attachments,
                                LastModifiedDate = x.LastModifiedDate,
                                CreatedDate = x.CreatedDate
                            })
                        };

                        return StatusCode((int)HttpStatusCode.OK, response);
                    }
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
            else
            {
                var serviceModel = new GetOrdersServiceModel
                {
                    Language = CultureInfo.CurrentCulture.Name,
                    SearchTerm = searchTerm,
                    PageIndex = pageIndex,
                    ItemsPerPage = itemsPerPage,
                    OrderBy = orderBy,
                    CreatedDateGreaterThan = createdDateGreaterThan,
                    OrderStatusId = orderStatusId,
                    Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                    IsSeller = User.IsInRole("Seller")
                };

                var validator = new GetOrdersModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var orders =  await _ordersService.GetAsync(serviceModel);

                    if (orders is not null)
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
                                BillingCountryId = x.BillingCountryId,
                                BillingFirstName = x.BillingFirstName,
                                BillingLastName = x.BillingLastName,
                                BillingPhoneNumber = x.BillingPhoneNumber,
                                BillingPostCode = x.BillingPostCode,
                                BillingRegion = x.BillingRegion,
                                BillingStreet = x.BillingStreet,
                                ShippingAddressId = x.ShippingAddressId,
                                ShippingCity = x.ShippingCity,
                                ShippingCompany = x.ShippingCompany,
                                ShippingPhoneNumber = x.ShippingPhoneNumber,
                                ShippingCountryId = x.ShippingCountryId,
                                ShippingFirstName = x.ShippingFirstName,
                                ShippingLastName = x.ShippingLastName,
                                ShippingPostCode = x.ShippingPostCode,
                                ShippingRegion = x.ShippingRegion,
                                ShippingStreet = x.ShippingStreet,
                                MoreInfo = x.MoreInfo,
                                Reason = x.Reason,
                                OrderStateId = x.OrderStateId,
                                OrderStatusId = x.OrderStatusId,
                                OrderStatusName = x.OrderStatusName,
                                OrderItems = x.OrderItems.Select(y => new OrderItemResponseModel
                                {
                                    Id = y.Id,
                                    OrderId = y.OrderId,
                                    ProductId = y.ProductId,
                                    ProductSku = y.ProductSku,
                                    ProductName = y.ProductName,
                                    PictureUrl = y.PictureUrl,
                                    Quantity = y.Quantity,
                                    StockQuantity = y.StockQuantity,
                                    OutletQuantity = y.OutletQuantity,
                                    UnitPrice = y.UnitPrice,
                                    Price = y.Price,
                                    Currency = y.Currency,
                                    ExternalReference = y.ExternalReference,
                                    MoreInfo = y.MoreInfo,
                                    OrderItemStateId = y.OrderItemStateId,
                                    OrderItemStatusId = y.OrderItemStatusId,
                                    OrderItemStatusName = y.OrderItemStatusName,
                                    LastOrderItemStatusChangeId = y.LastOrderItemStatusChangeId,
                                    OrderItemStatusChangeComment = y.OrderItemStatusChangeComment,
                                    LastModifiedDate = y.LastModifiedDate,
                                    CreatedDate = y.CreatedDate
                                }),
                                Attachments = x.Attachments,
                                LastModifiedDate = x.LastModifiedDate,
                                CreatedDate = x.CreatedDate
                            })
                        };

                        return StatusCode((int)HttpStatusCode.OK, response);
                    }
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
        }

        /// <summary>
        /// Gets order by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The order.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Get(Guid? id)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetOrderServiceModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                IsSeller = User.IsInRole("Seller")
            };

            var validator = new GetOrderModelValidator();

            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var order = await _ordersService.GetAsync(serviceModel);

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
                        BillingCountryId = order.BillingCountryId,
                        BillingFirstName = order.BillingFirstName,
                        BillingLastName = order.BillingLastName,
                        BillingPhoneNumber = order.BillingPhoneNumber,
                        BillingPostCode = order.BillingPostCode,
                        BillingRegion = order.BillingRegion,
                        BillingStreet = order.BillingStreet,
                        ShippingAddressId = order.ShippingAddressId,
                        ShippingCity = order.ShippingCity,
                        ShippingCompany = order.ShippingCompany,
                        ShippingPhoneNumber = order.ShippingPhoneNumber,
                        ShippingCountryId = order.ShippingCountryId,
                        ShippingFirstName = order.ShippingFirstName,
                        ShippingLastName = order.ShippingLastName,
                        ShippingPostCode = order.ShippingPostCode,
                        ShippingRegion = order.ShippingRegion,
                        ShippingStreet = order.ShippingStreet,
                        MoreInfo = order.MoreInfo,
                        Reason = order.Reason,
                        OrderStateId = order.OrderStateId,
                        OrderStatusId = order.OrderStatusId,
                        OrderStatusName = order.OrderStatusName,
                        OrderItems = order.OrderItems.OrEmptyIfNull().Select(x => new OrderItemResponseModel
                        {
                            Id = x.Id, 
                            OrderId = x.OrderId,
                            OrderItemStateId = x.OrderItemStateId,
                            OrderItemStatusId = x.OrderItemStatusId,
                            OrderItemStatusName = x.OrderItemStatusName,
                            OrderItemStatusChangeComment = x.OrderItemStatusChangeComment,
                            LastOrderItemStatusChangeId = x.LastOrderItemStatusChangeId,
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
                            MoreInfo = x.MoreInfo,
                            LastModifiedDate = x.LastModifiedDate,
                            CreatedDate = x.CreatedDate
                        }),
                        Attachments = order.Attachments,
                        LastModifiedDate = order.LastModifiedDate,
                        CreatedDate = order.CreatedDate
                    };

                    return StatusCode((int)HttpStatusCode.OK, response);
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.NoContent);
                }

            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Gets order item by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The order item.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("orderitems/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> GetOrderItem(Guid? id)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetOrderItemServiceModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value
            };

            var validator = new GetOrderItemModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var orderItem = await _ordersService.GetAsync(serviceModel);

                if (orderItem is not null)
                {
                    var response = new OrderItemResponseModel
                    {
                        Id = orderItem.Id,
                        OrderId = orderItem.OrderId,
                        ProductId = orderItem.ProductId,
                        ProductSku = orderItem.ProductSku,
                        ProductName = orderItem.ProductName,
                        PictureUrl = orderItem.PictureUrl,
                        Quantity = orderItem.Quantity,
                        StockQuantity = orderItem.StockQuantity,
                        UnitPrice = orderItem.UnitPrice,
                        Price = orderItem.Price,
                        Currency = orderItem.Currency,
                        OutletQuantity = orderItem.OutletQuantity,
                        ExternalReference = orderItem.ExternalReference,
                        MoreInfo = orderItem.MoreInfo,
                        OrderItemStateId = orderItem.OrderItemStateId,
                        OrderItemStatusId = orderItem.OrderItemStatusId,
                        OrderItemStatusName = orderItem.OrderItemStatusName,
                        OrderItemStatusChangeComment = orderItem.OrderItemStatusChangeComment,
                        LastOrderItemStatusChangeId = orderItem.LastOrderItemStatusChangeId,
                        LastModifiedDate = orderItem.LastModifiedDate,
                        CreatedDate = orderItem.CreatedDate
                    };

                    return StatusCode((int)HttpStatusCode.OK, response);
                }
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Gets order item statuses history by order item id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The order item statuses.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("orderitemstatuses/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> GetOrderItemStatuses(Guid? id)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetOrderItemStatusChangesServiceModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value
            };

            var validator = new GetOrderItemStatusChangesModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var statusChanges = await _ordersService.GetAsync(serviceModel);

                if (statusChanges is not null)
                {
                    var response = new OrderItemStatusChangesResponseModel
                    {
                        OrderItemId = statusChanges.OrderItemId,
                        StatusChanges = statusChanges.OrderItemStatusChanges.OrEmptyIfNull().Select(x => new OrderItemStatusChangeResponseModel { 
                            OrderItemStateId = x.OrderItemStateId,
                            OrderItemStatusId = x.OrderItemStatusId,
                            OrderItemStatusName = x.OrderItemStatusName,
                            OrderItemStatusChangeComment = x.OrderItemStatusChangeComment,
                            CreatedDate = x.CreatedDate
                        })
                    };

                    return StatusCode((int)HttpStatusCode.OK, response);
                }
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        ///  Updates the order item status.
        /// </summary>
        /// <returns>The updated order item status.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [Route("orderitemstatus")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Status(UpdateOrderItemStatusRequestModel request)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new UpdateOrderItemStatusServiceModel
            {
                Id = request.Id,
                OrderItemStatusId = request.OrderItemStatusId,
                OrderItemStatusChangeComment = request.ExpectedDateOfProductOnStock,
                Language = CultureInfo.CurrentCulture.Name,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value
            };

            var validator = new UpdateOrderItemStatusModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                await _ordersService.UpdateOrderItemStatusAsync(serviceModel);

                return StatusCode((int)HttpStatusCode.OK);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        ///  Updates the order items statuses.
        /// </summary>
        /// <returns>The updated order item status.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [Route("orderitemsstatuses")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Sync(SyncOrderItemsStatusesRequestModel request)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new UpdateOrderItemsStatusesServiceModel
            {
                Language = CultureInfo.CurrentCulture.Name,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrderItems = request.OrderItems.OrEmptyIfNull().Select(x => new UpdateOrderItemsStatusServiceModel
                {
                    Id = x.Id,
                    StatusId = x.StatusId,
                    StatusChangeComment = x.StatusChangeComment,
                    Language = x.Language
                })
            };

            var validator = new UpdateOrderItemsStatusesModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                await _ordersService.SyncOrderItemsStatusesAsync(serviceModel);

                return StatusCode((int)HttpStatusCode.OK);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        ///  Updates the order line items statuses.
        /// </summary>
        /// <returns>The updated order line status.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [Route("orderlinesstatuses")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<OrderLineUpdatedStatusResponseModel>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> SyncOrderLines(SyncOrderLinesStatusesRequestModel request)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new UpdateOrderLinesStatusesServiceModel
            {
                Language = CultureInfo.CurrentCulture.Name,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrderItems = request.OrderItems.OrEmptyIfNull().Select(x => new UpdateOrderLinesStatusServiceModel
                {
                    Id = x.OrderId,
                    OrderLineIndex = x.OrderLineIndex,
                    StatusId = x.StatusId,
                    CommentTranslations = x.CommentTranslations.OrEmptyIfNull().Select(x => new UpdateOrderLineCommentServiceModel 
                    { 
                        Text = x.Text,
                        Language = x.Language
                    })
                })
            };

            var validator = new UpdateOrderLinesStatusesModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var updatedStatuses = await _ordersService.SyncOrderLinesStatusesAsync(serviceModel);

                return StatusCode((int)HttpStatusCode.OK, updatedStatuses.Select(x => new OrderLineUpdatedStatusResponseModel
                {
                    OrderId = x.OrderId,
                    OrderLineIndex = x.OrderLineIndex,
                    PreviousStateId = x.PreviousStateId,
                    PreviousStatusId = x.PreviousStatusId,
                    NewStateId = x.NewStateId,
                    NewStatusId = x.NewStatusId
                }));
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
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Post(UpdateOrderStatusRequestModel model)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new UpdateOrderStatusServiceModel
            {
                OrderId = model.OrderId,
                OrderStatusId = model.OrderStatusId,
                Language = CultureInfo.CurrentCulture.Name,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                IsSeller = User.IsInRole("Seller")
            };

            var validator = new UpdateOrderStatusModelValidator();

            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var order = await _ordersService.SaveOrderStatusAsync(serviceModel);

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
                        BillingCountryId = order.BillingCountryId,
                        BillingFirstName = order.BillingFirstName,
                        BillingLastName = order.BillingLastName,
                        BillingPhoneNumber = order.BillingPhoneNumber,
                        BillingPostCode = order.BillingPostCode,
                        BillingRegion = order.BillingRegion,
                        BillingStreet = order.BillingStreet,
                        ShippingAddressId = order.ShippingAddressId,
                        ShippingCity = order.ShippingCity,
                        ShippingCompany = order.ShippingCompany,
                        ShippingPhoneNumber = order.ShippingPhoneNumber,
                        ShippingCountryId = order.ShippingCountryId,
                        ShippingFirstName = order.ShippingFirstName,
                        ShippingLastName = order.ShippingLastName,
                        ShippingPostCode = order.ShippingPostCode,
                        ShippingRegion = order.ShippingRegion,
                        ShippingStreet = order.ShippingStreet,
                        MoreInfo = order.MoreInfo,
                        Reason = order.Reason,
                        OrderStateId = order.OrderStateId,
                        OrderStatusId = order.OrderStatusId,
                        OrderStatusName = order.OrderStatusName,
                        OrderItems = order.OrderItems.OrEmptyIfNull().Select(x => new OrderItemResponseModel
                        {
                            ProductId = x.ProductId,
                            ProductSku = x.ProductSku,
                            ProductName = x.ProductName,
                            PictureUrl = x.PictureUrl,
                            Quantity = x.Quantity,
                            OrderItemStateId = x.OrderItemStateId,
                            OrderItemStatusId = x.OrderItemStatusId,
                            OrderItemStatusName = x.OrderItemStatusName,
                            OrderItemStatusChangeComment = x.OrderItemStatusChangeComment,
                            StockQuantity = x.StockQuantity,
                            OutletQuantity = x.OutletQuantity,
                            UnitPrice = x.UnitPrice,
                            Price = x.Price,
                            Currency = x.Currency,
                            ExternalReference = x.ExternalReference,
                            MoreInfo = x.MoreInfo,
                            LastModifiedDate = x.LastModifiedDate,
                            CreatedDate = x.CreatedDate
                        }),
                        Attachments = order.Attachments,
                        LastModifiedDate = order.LastModifiedDate,
                        CreatedDate = order.CreatedDate
                    };

                    return StatusCode((int)HttpStatusCode.OK, response);
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.NoContent);
                }
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }
    }
}
