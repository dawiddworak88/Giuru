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
using Newtonsoft.Json;
using Basket.Api.Infrastructure;
using Basket.Api.Infrastructure.Entities;
using Foundation.Extensions.Exceptions;
using System.Net;

namespace Basket.Api.Services
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository basketRepository;
        private readonly IEventBus eventBus;
        private readonly BasketContext basketContext;

        public BasketService(
            IBasketRepository basketRepository,
            BasketContext basketContext,
            IEventBus eventBus)
        {
            this.basketRepository = basketRepository;
            this.eventBus = eventBus;
            this.basketContext = basketContext;
        }

        public async Task CheckoutAsync(CheckoutBasketServiceModel checkoutBasketServiceModel)
        {
            var basket = await this.basketRepository.GetBasketAsync(checkoutBasketServiceModel.BasketId.Value);

            if (basket == null)
            {
                throw new ArgumentNullException();
            }

            var message = new BasketCheckoutAcceptedIntegrationEvent
            {
                Language = checkoutBasketServiceModel.Language,
                OrganisationId = checkoutBasketServiceModel.OrganisationId,
                Username = checkoutBasketServiceModel.Username,
                ClientId = checkoutBasketServiceModel.ClientId,
                SellerId = checkoutBasketServiceModel.OrganisationId,
                ClientName = checkoutBasketServiceModel.ClientName,
                BillingAddressId = checkoutBasketServiceModel.BillingAddressId,
                BillingCity = checkoutBasketServiceModel.BillingCity,
                BillingCompany = checkoutBasketServiceModel.BillingCompany,
                BillingCountryCode = checkoutBasketServiceModel.BillingCountryCode,
                BillingFirstName = checkoutBasketServiceModel.BillingFirstName,
                BillingLastName = checkoutBasketServiceModel.BillingLastName,
                BillingPhone = checkoutBasketServiceModel.BillingPhone,
                BillingPhonePrefix = checkoutBasketServiceModel.BillingPhonePrefix,
                BillingPostCode = checkoutBasketServiceModel.BillingPostCode,
                BillingRegion = checkoutBasketServiceModel.BillingRegion,
                BillingStreet = checkoutBasketServiceModel.BillingStreet,
                ShippingAddressId = checkoutBasketServiceModel.ShippingAddressId,
                ShippingCity = checkoutBasketServiceModel.ShippingCity,
                ShippingCompany = checkoutBasketServiceModel.ShippingCompany,
                ShippingCountryCode = checkoutBasketServiceModel.ShippingCountryCode,
                ShippingFirstName = checkoutBasketServiceModel.ShippingFirstName,
                ShippingLastName = checkoutBasketServiceModel.ShippingLastName,
                ShippingPhone = checkoutBasketServiceModel.ShippingPhone,
                ShippingPhonePrefix = checkoutBasketServiceModel.ShippingPhonePrefix,
                ShippingPostCode = checkoutBasketServiceModel.ShippingPostCode,
                ShippingRegion = checkoutBasketServiceModel.ShippingRegion,
                ShippingStreet = checkoutBasketServiceModel.ShippingStreet,
                ExternalReference = checkoutBasketServiceModel.ExternalReference,
                ExpectedDeliveryDate = checkoutBasketServiceModel.ExpectedDeliveryDate,
                MoreInfo = checkoutBasketServiceModel.MoreInfo,
                Basket = new BasketEventModel
                {
                    Id = basket.Id,
                    Items = basket.Items.Select(x => new BasketItemEventModel 
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
                }
            };

            var deleteModel = new DeleteBasketServiceModel
            {
                OrganisationId = checkoutBasketServiceModel.OrganisationId
            };

            if (!checkoutBasketServiceModel.isSeller)
            {
                await this.DeleteAsync(deleteModel);
            }

            this.eventBus.Publish(message);
        }

        public async Task DeleteAsync(DeleteBasketServiceModel serviceModel)
        {
            var basket = from b in this.basketContext.Baskets where b.OwnerId == serviceModel.OrganisationId.Value select b;
            if (basket == null)
            {
                throw new CustomException("InventoryNotFound", (int)HttpStatusCode.NotFound);
            }

            this.basketContext.RemoveRange(basket);
            await this.basketContext.SaveChangesAsync();
        }

        public async Task<BasketOrderServiceModel> DelteItemAsync(DeleteBasketItemServiceModel serviceModel)
        {
            var basket = from b in this.basketContext.Baskets where b.OwnerId == serviceModel.OrganisationId.Value select b;
            if (basket == null)
            {
                throw new CustomException("InventoryNotFound", (int)HttpStatusCode.NotFound);
            }

            var basketItem = basket.FirstOrDefault(x => x.Id == serviceModel.Id.Value);
            var message = new BasketProductBookingIntegrationEvent
            {
                ProductId = basketItem.ProductId.Value,
                ProductSku = basketItem.ProductSku,
                BookedQuantity = -basketItem.Quantity
            };

            this.eventBus.Publish(message);
            this.basketContext.Baskets.Remove(basketItem);
            await this.basketContext.SaveChangesAsync();

            if (basket.OrEmptyIfNull().Any())
            {
                var basketItems = new BasketOrderServiceModel
                {
                    Id = basket.FirstOrDefault().Id,
                    OwnerId = basket.FirstOrDefault().OwnerId,
                    Items = basket.Select(x => new BasketOrderItemServiceModel
                    {
                        Id = x.Id,
                        ProductId = x.ProductId,
                        Name = x.ProductName,
                        Sku = x.ProductSku,
                        ImageSrc = x.PictureUrl,
                        ImageAlt = x.ProductName,
                        Quantity = x.Quantity,
                        ExternalReference = x.ExternalReference,
                        DeliveryFrom = x.DeliveryFrom,
                        DeliveryTo = x.DeliveryTo,
                        MoreInfo = x.MoreInfo
                    })
                };

                return basketItems;
            }
            return default;
        }

        public async Task<BasketOrderServiceModel> GetByOrganisation(GetBasketByOrganisationServiceModel serviceModel)
        {
            var basket = from b in this.basketContext.Baskets
                         where b.OwnerId == serviceModel.OrganisationId.Value
                         select new
                         {
                             Id = b.Id,
                             OwnerId = b.OwnerId,
                             ProductId = b.ProductId,
                             ProductName = b.ProductName,
                             ProductSku = b.ProductSku,
                             PictureUrl = b.PictureUrl,
                             Quantity = b.Quantity,
                             ExternalReference = b.ExternalReference,
                             DeliveryFrom = b.DeliveryFrom,
                             DeliveryTo = b.DeliveryTo,
                             MoreInfo = b.MoreInfo
                         };

            if (basket.OrEmptyIfNull().Any())
            {
                var basketItems = new BasketOrderServiceModel
                {
                    Id = basket.FirstOrDefault().Id,
                    OwnerId = basket.FirstOrDefault().OwnerId,
                    Items = basket.Select(x => new BasketOrderItemServiceModel
                    {
                        Id = x.Id,
                        ProductId = x.ProductId,
                        Name = x.ProductName,
                        Sku = x.ProductSku,
                        ImageSrc = x.PictureUrl,
                        ImageAlt = x.ProductName,
                        Quantity = x.Quantity,
                        ExternalReference = x.ExternalReference,
                        DeliveryFrom = x.DeliveryFrom,
                        DeliveryTo = x.DeliveryTo,
                        MoreInfo = x.MoreInfo
                    })
                };

                return basketItems;
            }
            return default;
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
                    ExternalReference = x.ExternalReference,
                    DeliveryFrom = x.DeliveryFrom,
                    DeliveryTo = x.DeliveryTo,
                    MoreInfo = x.MoreInfo
                })
            };

            if (!serviceModel.IsSeller)
            {
                var basketItem = new BasketItems
                {
                    BasketId = serviceModel.Id,
                    OwnerId = serviceModel.OrganisationId.Value,
                    ProductId = serviceModel.Items.LastOrDefault().ProductId,
                    ProductSku = serviceModel.Items.LastOrDefault().ProductSku,
                    ProductName = serviceModel.Items.LastOrDefault().ProductName,
                    PictureUrl = serviceModel.Items.LastOrDefault().PictureUrl,
                    Quantity = (int)serviceModel.Items.LastOrDefault().Quantity,
                    ExternalReference = serviceModel.Items.LastOrDefault().ExternalReference,
                    DeliveryFrom = serviceModel.Items.LastOrDefault().DeliveryFrom,
                    DeliveryTo = serviceModel.Items.LastOrDefault().DeliveryTo,
                    MoreInfo = serviceModel.Items.LastOrDefault().MoreInfo
                };

                var message = new BasketProductBookingIntegrationEvent
                {
                    ProductId = serviceModel.Items.LastOrDefault().ProductId.Value,
                    ProductSku = serviceModel.Items.LastOrDefault().ProductSku,
                    BookedQuantity = (int)serviceModel.Items.LastOrDefault().Quantity
                };

                this.basketContext.Baskets.Add(basketItem);
                this.eventBus.Publish(message);
                await this.basketContext.SaveChangesAsync();
            }
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
                    ExternalReference = x.ExternalReference,
                    DeliveryFrom = x.DeliveryFrom,
                    DeliveryTo = x.DeliveryTo,
                    MoreInfo = x.MoreInfo
                })
            };
        }
    }
}
