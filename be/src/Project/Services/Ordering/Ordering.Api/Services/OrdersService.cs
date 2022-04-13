using Foundation.EventBus.Abstractions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Extensions;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Ordering.Api.Infrastructure;
using Ordering.Api.Infrastructure.Orders.Definitions;
using Ordering.Api.Infrastructure.Orders.Entities;
using Ordering.Api.IntegrationEvents;
using Ordering.Api.ServicesModels;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Ordering.Api.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly OrderingContext context;
        private readonly IEventBus eventBus;
        private readonly IStringLocalizer<OrderResources> orderLocalizer;

        public OrdersService(
            OrderingContext context,
            IEventBus eventBus,
            IStringLocalizer<OrderResources> orderLocalizer)
        {
            this.context = context;
            this.eventBus = eventBus;
            this.orderLocalizer = orderLocalizer;
        }

        public async Task CheckoutAsync(CheckoutBasketServiceModel serviceModel)
        {
            var order = new Order
            {
                OrderStateId = OrderStatesConstants.NewId,
                OrderStatusId = OrderStatusesConstants.NewId,
                ClientId = serviceModel.ClientId,
                ClientName = serviceModel.ClientName,
                SellerId = serviceModel.SellerId,
                BillingAddressId = serviceModel.BillingAddressId,
                BillingCity = serviceModel.BillingCity,
                BillingCompany = serviceModel.BillingCompany,
                BillingCountryCode = serviceModel.BillingCountryCode,
                BillingFirstName = serviceModel.BillingFirstName,
                BillingLastName = serviceModel.BillingLastName,
                BillingPhone = serviceModel.BillingPhone,
                BillingPhonePrefix = serviceModel.BillingPhonePrefix,
                BillingPostCode = serviceModel.BillingPostCode,
                BillingRegion = serviceModel.BillingRegion,
                BillingStreet = serviceModel.BillingStreet,
                ShippingAddressId = serviceModel.ShippingAddressId,
                ShippingCity = serviceModel.ShippingCity,
                ShippingCompany = serviceModel.ShippingCompany,
                ShippingCountryCode = serviceModel.ShippingCountryCode,
                ShippingFirstName = serviceModel.ShippingFirstName,
                ShippingLastName = serviceModel.ShippingLastName,
                ShippingPhone = serviceModel.ShippingPhone,
                ShippingPhonePrefix = serviceModel.ShippingPhonePrefix,
                ShippingPostCode = serviceModel.ShippingPostCode,
                ShippingRegion = serviceModel.ShippingRegion,
                ShippingStreet = serviceModel.ShippingStreet,
                ExternalReference = serviceModel.ExternalReference,
                ExpectedDeliveryDate = serviceModel.ExpectedDeliveryDate,
                MoreInfo = serviceModel.MoreInfo,
                IpAddress = serviceModel.IpAddress
            };

            this.context.Orders.Add(order.FillCommonProperties());

            foreach (var basketItem in serviceModel.Items)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = basketItem.ProductId.Value,
                    ProductSku = basketItem.ProductSku,
                    ProductName = basketItem.ProductName,
                    PictureUrl = basketItem.PictureUrl,
                    Quantity = basketItem.Quantity,
                    ExternalReference = basketItem.ExternalReference,
                    ExpectedDeliveryFrom = basketItem.ExpectedDeliveryFrom,
                    ExpectedDeliveryTo = basketItem.ExpectedDeliveryTo,
                    MoreInfo = basketItem.MoreInfo                    
                };

                this.context.OrderItems.Add(orderItem.FillCommonProperties());
            }

            await this.context.SaveChangesAsync();

            var message = new OrderStartedIntegrationEvent
            { 
                BasketId =  serviceModel.BasketId
            };

            this.eventBus.Publish(message);
        }

        public async Task<PagedResults<IEnumerable<OrderServiceModel>>> GetAsync(GetOrdersServiceModel model)
        {
            var orders = from c in this.context.Orders
                         join os in this.context.OrderStatuses on c.OrderStatusId equals os.Id
                         join ost in this.context.OrderStatusTranslations on os.Id equals ost.OrderStatusId
                         where ost.Language == model.Language && c.IsActive
                         select new OrderServiceModel
                         {
                             Id = c.Id,
                             SellerId = c.SellerId,
                             ClientId = c.ClientId.Value,
                             ClientName = c.ClientName,
                             BillingAddressId = c.BillingAddressId,
                             BillingCity = c.BillingCity,
                             BillingCompany = c.BillingCompany,
                             BillingCountryCode = c.BillingCountryCode,
                             BillingFirstName = c.BillingFirstName,
                             BillingLastName = c.BillingLastName,
                             BillingPhone = c.BillingPhone,
                             BillingPhonePrefix = c.BillingPhonePrefix,
                             BillingPostCode = c.BillingPostCode,
                             BillingRegion = c.BillingRegion,
                             BillingStreet = c.BillingStreet,
                             ShippingAddressId = c.ShippingAddressId,
                             ShippingCity = c.ShippingCity,
                             ShippingCompany = c.ShippingCompany,
                             ShippingCountryCode = c.ShippingCountryCode,
                             ShippingFirstName = c.ShippingFirstName,
                             ShippingLastName = c.ShippingLastName,
                             ShippingPhone = c.ShippingPhone,
                             ShippingPhonePrefix = c.ShippingPhonePrefix,
                             ShippingPostCode = c.ShippingPostCode,
                             ShippingRegion = c.ShippingRegion,
                             ShippingStreet = c.ShippingStreet,
                             ExpectedDeliveryDate = c.ExpectedDeliveryDate,
                             ExternalReference = c.ExternalReference,
                             MoreInfo = c.MoreInfo,
                             Reason = c.Reason,
                             OrderStateId = c.OrderStateId,
                             OrderStatusId = c.OrderStatusId,
                             OrderStatusName = ost.Name,
                             OrderItems = c.OrderItems.Select(x => new OrderItemServiceModel
                             {
                                 ProductId = x.ProductId,
                                 ProductSku = x.ProductSku,
                                 ProductName = x.ProductName,
                                 PictureUrl = x.PictureUrl,
                                 Quantity = x.Quantity,
                                 ExternalReference = x.ExternalReference,
                                 ExpectedDeliveryFrom = x.ExpectedDeliveryFrom,
                                 ExpectedDeliveryTo = x.ExpectedDeliveryTo,
                                 MoreInfo = x.MoreInfo,
                                 LastModifiedDate = x.LastModifiedDate,
                                 CreatedDate = x.CreatedDate
                             }),
                             LastModifiedDate = c.LastModifiedDate,
                             CreatedDate = c.CreatedDate
                         };

            if (model.IsSeller is false)
            {
                orders = orders.Where(x => x.SellerId == model.OrganisationId.Value || x.OrderItems.Any(y => y.ExternalReference.StartsWith(model.SearchTerm)) || x.Id.ToString() == model.SearchTerm);
            }

            if (!string.IsNullOrWhiteSpace(model.SearchTerm))
            {
                orders = orders.Where(x => x.ClientName.StartsWith(model.SearchTerm) || x.OrderItems.Any(y => y.ExternalReference.StartsWith(model.SearchTerm)) || x.Id.ToString() == model.SearchTerm);
            }

            if (model.CreatedDateGreaterThan.HasValue)
            {
                orders = orders.Where(x => x.CreatedDate >= model.CreatedDateGreaterThan);
            }

            orders = orders.ApplySort(model.OrderBy);

            return orders.PagedIndex(new Pagination(orders.Count(), model.ItemsPerPage), model.PageIndex);
        }

        public async Task<OrderServiceModel> GetAsync(GetOrderServiceModel model)
        {
            var orders = from c in this.context.Orders
                         join os in this.context.OrderStatuses on c.OrderStatusId equals os.Id
                         join ost in this.context.OrderStatusTranslations on os.Id equals ost.OrderStatusId
                         where c.Id == model.Id && ost.Language == model.Language && c.IsActive
                         select new OrderServiceModel
                          {
                              Id = c.Id,
                              SellerId = c.SellerId,
                              ClientId = c.ClientId.Value,
                              ClientName = c.ClientName,
                              BillingAddressId = c.BillingAddressId,
                              BillingCity = c.BillingCity,
                              BillingCompany = c.BillingCompany,
                              BillingCountryCode = c.BillingCountryCode,
                              BillingFirstName = c.BillingFirstName,
                              BillingLastName = c.BillingLastName,
                              BillingPhone = c.BillingPhone,
                              BillingPhonePrefix = c.BillingPhonePrefix,
                              BillingPostCode = c.BillingPostCode,
                              BillingRegion = c.BillingRegion,
                              BillingStreet = c.BillingStreet,
                              ShippingAddressId = c.ShippingAddressId,
                              ShippingCity = c.ShippingCity,
                              ShippingCompany = c.ShippingCompany,
                              ShippingCountryCode = c.ShippingCountryCode,
                              ShippingFirstName = c.ShippingFirstName,
                              ShippingLastName = c.ShippingLastName,
                              ShippingPhone = c.ShippingPhone,
                              ShippingPhonePrefix = c.ShippingPhonePrefix,
                              ShippingPostCode = c.ShippingPostCode,
                              ShippingRegion = c.ShippingRegion,
                              ShippingStreet = c.ShippingStreet,
                              ExternalReference = c.ExternalReference,
                              ExpectedDeliveryDate = c.ExpectedDeliveryDate,
                              MoreInfo = c.MoreInfo,
                              Reason = c.Reason,
                              OrderStateId = c.OrderStateId,
                              OrderStatusId = c.OrderStatusId,
                              OrderStatusName = ost.Name,
                              OrderItems = c.OrderItems.Select(x => new OrderItemServiceModel 
                              { 
                                ProductId = x.ProductId,
                                ProductSku = x.ProductSku,
                                ProductName = x.ProductName,
                                PictureUrl = x.PictureUrl,
                                Quantity = x.Quantity,
                                ExternalReference = x.ExternalReference,
                                ExpectedDeliveryFrom = x.ExpectedDeliveryFrom,
                                ExpectedDeliveryTo = x.ExpectedDeliveryTo,
                                MoreInfo = x.MoreInfo,
                                LastModifiedDate = x.LastModifiedDate,
                                CreatedDate = x.CreatedDate
                              }),
                              LastModifiedDate = c.LastModifiedDate,
                              CreatedDate = c.CreatedDate
                          };

            if (model.IsSeller is false)
            {
                orders = orders.Where(x => x.SellerId == model.OrganisationId.Value);
            }

            return await orders.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<OrderStatusServiceModel>> GetOrderStatusesAsync(GetOrderStatusesServiceModel serviceModel)
        {
            var orderStatuses = from orderstatus in this.context.OrderStatuses
                                join orderstate in this.context.OrderStates on orderstatus.OrderStateId equals orderstate.Id
                                join orderstatustranslation in this.context.OrderStatusTranslations on orderstatus.Id equals orderstatustranslation.OrderStatusId
                                where orderstatustranslation.Language == serviceModel.Language && orderstatus.IsActive
                                orderby orderstatus.Order
                                select new OrderStatusServiceModel
                                {
                                    Id = orderstatus.Id,
                                    OrderStateId = orderstate.Id,
                                    Name = orderstatustranslation.Name,
                                    LastModifiedDate = orderstatus.LastModifiedDate,
                                    CreatedDate = orderstatus.CreatedDate
                                };

            return orderStatuses;
        }

        public async Task<OrderServiceModel> SaveOrderStatusAsync(UpdateOrderStatusServiceModel serviceModel)
        {
            var orders = this.context.Orders.Where(x => x.Id == serviceModel.OrderId && x.IsActive);

            if (serviceModel.IsSeller is false)
            {
                orders = orders.Where(x => x.SellerId == serviceModel.OrganisationId.Value);
            }

            var order = await orders.FirstOrDefaultAsync();

            if (order == null)
            {
                throw new CustomException(this.orderLocalizer.GetString("OrderNotFound"), (int)HttpStatusCode.NotFound);
            }

            var newOrderOstatus = await this.context.OrderStatuses.FirstOrDefaultAsync(x => x.Id == serviceModel.OrderStatusId && x.IsActive);

            if (newOrderOstatus == null)
            {
                throw new CustomException(this.orderLocalizer.GetString("OrderStatusNotFound"), (int)HttpStatusCode.NotFound);
            }

            order.OrderStatusId = newOrderOstatus.Id;
            order.OrderStateId = newOrderOstatus.OrderStateId;

            await this.context.SaveChangesAsync();

            return await this.GetAsync(new GetOrderServiceModel {  
                Id = serviceModel.OrderId,
                OrganisationId = serviceModel.OrganisationId,
                Username = serviceModel.Username,
                Language = serviceModel.Language,
                IsSeller = serviceModel.IsSeller
            });
        }
    }
}
