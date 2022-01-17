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

            this.eventBus.Publish(message);
            await this.basketRepository.DeleteBasketAsync(checkoutBasketServiceModel.BasketId.Value);
        }

        public async Task DeleteAsync(DeleteBasketServiceModel serviceModel)
        {
            await this.basketRepository.DeleteBasketAsync(serviceModel.OrganisationId.Value);
        }

        public async Task<BasketServiceModel> GetByOrganisation(GetBasketByOrganisationServiceModel serviceModel)
        {
            var basket = await this.basketRepository.GetBasketAsync(serviceModel.OrganisationId.Value);
            if (basket == null)
            {
                var emptyBasket = new BasketServiceModel()
                {
                    Id = serviceModel.OrganisationId.Value,
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
                    ExternalReference = x.ExternalReference,
                    DeliveryFrom = x.DeliveryFrom,
                    DeliveryTo = x.DeliveryTo,
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
                    ExternalReference = x.ExternalReference,
                    DeliveryFrom = x.DeliveryFrom,
                    DeliveryTo = x.DeliveryTo,
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
                    ExternalReference = x.ExternalReference,
                    DeliveryFrom = x.DeliveryFrom,
                    DeliveryTo = x.DeliveryTo,
                    MoreInfo = x.MoreInfo
                })
            };
        }
    }
}
