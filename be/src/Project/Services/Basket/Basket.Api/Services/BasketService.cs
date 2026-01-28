using Basket.Api.ServicesModels;
using Basket.Api.Repositories;
using Foundation.EventBus.Abstractions;
using Foundation.Extensions.ExtensionMethods;
using System;
using System.Linq;
using System.Threading.Tasks;
using Basket.Api.RepositoriesModels;
using Basket.Api.IntegrationEvents;
using Basket.Api.IntegrationEventsModels;
using System.Collections.Generic;
using System.Diagnostics;

namespace Basket.Api.Services
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository basketRepository;
        private readonly IEventBus eventBus;

        public BasketService(
            IBasketRepository basketRepository,
            IEventBus eventBus)
        {
            this.basketRepository = basketRepository;
            this.eventBus = eventBus;
        }

        public async Task CheckoutAsync(CheckoutBasketServiceModel checkoutBasketServiceModel)
        {
            using var source = new ActivitySource(this.GetType().Name);

            var message = new BasketCheckoutAcceptedIntegrationEvent
            {
                BasketId = checkoutBasketServiceModel.BasketId,
                Language = checkoutBasketServiceModel.Language,
                OrganisationId = checkoutBasketServiceModel.OrganisationId,
                Username = checkoutBasketServiceModel.Username,
                ClientId = checkoutBasketServiceModel.ClientId,
                SellerId = checkoutBasketServiceModel.OrganisationId,
                ClientName = checkoutBasketServiceModel.ClientName,
                ClientEmail = checkoutBasketServiceModel.ClientEmail,
                BillingAddressId = checkoutBasketServiceModel.BillingAddressId,
                BillingCity = checkoutBasketServiceModel.BillingCity,
                BillingCompany = checkoutBasketServiceModel.BillingCompany,
                BillingCountryId = checkoutBasketServiceModel.BillingCountryId,
                BillingFirstName = checkoutBasketServiceModel.BillingFirstName,
                BillingLastName = checkoutBasketServiceModel.BillingLastName,
                BillingPhoneNumber = checkoutBasketServiceModel.BillingPhoneNumber,
                BillingPostCode = checkoutBasketServiceModel.BillingPostCode,
                BillingRegion = checkoutBasketServiceModel.BillingRegion,
                BillingStreet = checkoutBasketServiceModel.BillingStreet,
                ShippingAddressId = checkoutBasketServiceModel.ShippingAddressId,
                ShippingCity = checkoutBasketServiceModel.ShippingCity,
                ShippingCompany = checkoutBasketServiceModel.ShippingCompany,
                ShippingCountryId = checkoutBasketServiceModel.ShippingCountryId,
                ShippingFirstName = checkoutBasketServiceModel.ShippingFirstName,
                ShippingLastName = checkoutBasketServiceModel.ShippingLastName,
                ShippingPhoneNumber = checkoutBasketServiceModel.ShippingPhoneNumber,
                ShippingPostCode = checkoutBasketServiceModel.ShippingPostCode,
                ShippingRegion = checkoutBasketServiceModel.ShippingRegion,
                ShippingStreet = checkoutBasketServiceModel.ShippingStreet,
                ExternalReference = checkoutBasketServiceModel.ExternalReference,
                MoreInfo = checkoutBasketServiceModel.MoreInfo,
                HasCustomOrder = checkoutBasketServiceModel.HasCustomOrder,
                HasApprovalToSendEmail = checkoutBasketServiceModel.HasApprovalToSendEmail,
                Attachments = checkoutBasketServiceModel.Attachments
            };

            var basket = await this.basketRepository.GetBasketAsync(checkoutBasketServiceModel.BasketId.Value);

            if (basket is not null)
            {
                message.Basket = new BasketEventModel
                {
                    Id = basket.Id,
                    Items = basket.Items.Select(x => new BasketItemEventModel
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
                        DeliveryFrom = x.DeliveryFrom,
                        DeliveryTo = x.DeliveryTo,
                        MoreInfo = x.MoreInfo
                    })
                };

                var itemGroups = basket.Items.OrEmptyIfNull().GroupBy(g => g.ProductId);
                var stockItems = new List<BasketCheckoutProductEventModel>();
                var outletItems = new List<BasketCheckoutProductEventModel>();

                foreach (var group in itemGroups)
                {
                    stockItems.Add(new BasketCheckoutProductEventModel
                    {
                        ProductId = group.FirstOrDefault().ProductId,
                        BookedQuantity = (int)group.Sum(x => x.StockQuantity)
                    });

                    outletItems.Add(new BasketCheckoutProductEventModel
                    {
                        ProductId = group.FirstOrDefault().ProductId,
                        BookedQuantity = (int)group.Sum(x => x.OutletQuantity)
                    });
                }

                var stockBookedItems = new BasketCheckoutStockProductsIntegrationEvent
                {
                    Items = stockItems.Select(x => new BasketCheckoutProductEventModel
                    {
                        ProductId = x.ProductId,
                        BookedQuantity = x.BookedQuantity
                    })
                };

                var outletBookedItems = new BasketCheckoutOutletProductsIntegrationEvent
                {
                    Items = outletItems.Select(x => new BasketCheckoutProductEventModel
                    {
                        ProductId = x.ProductId,
                        BookedQuantity = x.BookedQuantity
                    })
                };

                using var outletBookedItemsActivity = source.StartActivity($"{System.Reflection.MethodBase.GetCurrentMethod().Name} {outletBookedItems.GetType().Name}");
                this.eventBus.Publish(outletBookedItems);

                using var stockBookedItemsActivity = source.StartActivity($"{System.Reflection.MethodBase.GetCurrentMethod().Name} {stockBookedItems.GetType().Name}");
                this.eventBus.Publish(stockBookedItems);
            }

            using var messageActivity = source.StartActivity($"{System.Reflection.MethodBase.GetCurrentMethod().Name} {message.GetType().Name}");
            this.eventBus.Publish(message);
        }

        public async Task DeleteAsync(DeleteBasketServiceModel serviceModel)
        {
            await this.basketRepository.DeleteBasketAsync(serviceModel.Id.Value);
        }

        public async Task<BasketServiceModel> GetBasketById(GetBasketByIdServiceModel serviceModel)
        {
            var basket = await this.basketRepository.GetBasketAsync(serviceModel.Id.Value);
            if (basket == null)
            {
                var emptyBasket = new BasketServiceModel
                {
                    Id = serviceModel.Id.Value,
                    Items = Array.Empty<BasketItemServiceModel>()
                };

                return emptyBasket;
            }

            var response = new BasketServiceModel
            {
                Id = basket.Id.Value,
                Items = basket.Items.OrEmptyIfNull().Select(x => new BasketItemServiceModel
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

            return response;
        }

        public async Task<BasketServiceModel> UpdateAsync(UpdateBasketServiceModel serviceModel)
        {
            var basketModel = new BasketRepositoryModel
            {
                Id = serviceModel.Id,
                Items = serviceModel.Items.OrEmptyIfNull().Select(x => new BasketItemRepositoryModel
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

            var result = await this.basketRepository.UpdateBasketAsync(basketModel);

            return new BasketServiceModel
            {
                Id = result.Id,
                Items = result.Items.OrEmptyIfNull().Select(x => new BasketItemServiceModel
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
        }
    }
}
