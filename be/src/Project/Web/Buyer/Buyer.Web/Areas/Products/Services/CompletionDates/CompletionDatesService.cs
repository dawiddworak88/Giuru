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

        public async Task<List<Product>> GetCompletionDatesAsync(string token, string language, Guid? clientId, List<Product> products)
        {
            var inStockProducts = await _inventoryRepository.GetAvailbleProductsInventoryByIds(token, language, products.Select(x => x.Id));

            foreach (var product in products.OrEmptyIfNull())
            {
                if (inStockProducts.Any(x => x.ProductId == product.Id))
                {
                    product.IsStock = true;
                }

                if (IsFastDeliveryEnabled(product))
                {
                    product.IsFastDelivery = true;
                }
            }

            var clientFieldValues = await GetClientFieldValuesAsync(token, language, clientId);

            products = await _completionDatesRepository.PostAsync(
                token,
                language,
                products,
                clientFieldValues,
                DateTime.Now);

            return products;
        }

        private async Task<List<ClientFieldValue>> GetClientFieldValuesAsync(string token, string language, Guid? id)
        {
            var client = await _clientsRepository.GetClientAsync(token, language, id);

            if (client is not null)
            {
                return await _clientsRepository.GetClientFieldValuesAsync(token, language, client.Id);
            }

            return null;
        }

        private bool IsFastDeliveryEnabled(Product product)
        {
            if (product.ProductAttributes.Any(x => x.Key == ProductConstants.Schema.FastDeliveryName) is false)
            {
                return false;
            }

            var fastDelivery = product.ProductAttributes.FirstOrDefault(x => x.Key == ProductConstants.Schema.FastDeliveryName);

            return Boolean.Parse(fastDelivery.Values.FirstOrDefault());
        }
    }
}
