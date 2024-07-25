using Buyer.Web.Areas.Products.DomainModels;
using Buyer.Web.Areas.Products.Repositories.Inventories;
using Buyer.Web.Areas.Products.Repositories.Products;
using Buyer.Web.Shared.Repositories.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Buyer.Web.Areas.Products.Repositories.CompletionDates;
using Buyer.Web.Shared.DomainModels.Clients;
using Foundation.Extensions.ExtensionMethods;
using Buyer.Web.Areas.Shared.Definitions.Products;
using Foundation.GenericRepository.Paginations;

namespace Buyer.Web.Areas.Products.Services.CompletionDates
{
    public class CompletionDatesService : ICompletionDatesService
    {
        private readonly IProductsRepository _productsRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IClientsRepository _clientsRepository;
        private readonly ICompletionDatesRepository _completionDatesRepository;

        public CompletionDatesService(
            IProductsRepository productsRepository,
            IInventoryRepository inventoryRepository,
            IClientsRepository clientsRepository,
            ICompletionDatesRepository completionDatesRepository)
        {
            _productsRepository = productsRepository;
            _inventoryRepository = inventoryRepository;
            _clientsRepository = clientsRepository;
            _completionDatesRepository = completionDatesRepository;
        }

        public async Task<Product> GetCompletionDateAsync(string token, string language, Guid? clientId, Product product)
        {
            var productsOnStock = await _inventoryRepository.GetAvailbleProductsInventory(language, PaginationConstants.DefaultPageIndex, PaginationConstants.DefaultPageSize, token);

            if (productsOnStock.Data.OrEmptyIfNull().Any(x => x.ProductId == product.Id))
            {
                product.IsStock = true;
            }

            if (IsFastDeliveryEnabled(product))
            {
                product.IsFastDelivery = true;
            }

            var clientFieldValues = await GetClientFieldValuesAsync(token, language, clientId);

            product = await _completionDatesRepository.PostAsync(
                token,
                language,
                product,
                clientFieldValues,
                DateTime.UtcNow);

            return product;
        }

        public async Task<IEnumerable<Product>> GetCompletionDatesAsync(string token, string language, Guid? clientId, IEnumerable<Product> products)
        {

            var productsOnStock = await _inventoryRepository.GetAvailbleProductsInventoryByIds(token, language, products.Select(x => x.Id));

            foreach (var item in products.OrEmptyIfNull())
            {
                if (productsOnStock.Any(x => x.ProductId == item.Id))
                {
                    item.IsStock = true;
                }

                if (IsFastDeliveryEnabled(item))
                {
                    item.IsFastDelivery = true;
                }
            }

            var clientFieldValues = await GetClientFieldValuesAsync(token, language, clientId);

            products = await _completionDatesRepository.PostAsync(
                token,
                language,
                products,
                clientFieldValues,
                DateTime.UtcNow);

            return products;
        }

        private async Task<IEnumerable<ClientFieldValue>> GetClientFieldValuesAsync(string token, string language, Guid? id)
        {
            var client = await _clientsRepository.GetClientAsync(token, language, id);

            if (client is not null)
            {
                return await _clientsRepository.GetClientFieldValuesAsync(token, language, client.Id);
            }

            return default;
        }

        private bool IsFastDeliveryEnabled(Product product)
        {
            if (product.ProductAttributes.Any(x => x.Key == ProductConstants.Schema.FastDeliveryName) is false)
            {
                return false;
            }

            var fastDelivery = product.ProductAttributes.FirstOrDefault(x => x.Key == ProductConstants.Schema.FastDeliveryName);

            if (fastDelivery.Values is not null)
            {
                return Boolean.Parse(fastDelivery.Values.FirstOrDefault());
            }

            return false;
        }
    }
}
