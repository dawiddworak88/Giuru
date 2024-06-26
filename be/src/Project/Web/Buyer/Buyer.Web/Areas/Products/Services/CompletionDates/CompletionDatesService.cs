using Buyer.Web.Areas.Products.DomainModels;
using Buyer.Web.Areas.Products.Repositories.Inventories;
using Buyer.Web.Areas.Products.Repositories.Products;
using Buyer.Web.Shared.Repositories.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Buyer.Web.Areas.Products.Repositories.CompletionDates;
using Buyer.Web.Shared.Definitions.CompletionDate;
using Buyer.Web.Shared.DomainModels.Clients;
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

        public async Task GetCompletionDatesAsync(string token, string language, List<Product> products, Guid? clientId)
        {
            var inStockProducts = await _inventoryRepository.GetAvailbleProductsInventoryByIds(token, language, products.Select(x => x.Id));

            var clientFieldValues = await GetClientFieldValuesAsync(token, language, clientId);

            var transportId = GetParameterValue(clientFieldValues, ClientFieldsConstants.Transport.Id);
            var zoneId = GetParameterValue(clientFieldValues, ClientFieldsConstants.Zone.Id);
            var campaignId = GetParameterValue(clientFieldValues, ClientFieldsConstants.Campaign.Id);

            foreach (var product in products)
            {
                var conditionId = ClientFieldsConstants.Condition.StandardId;

                if (IsFastDeliveryEnable(product))
                {
                    conditionId = ClientFieldsConstants.Condition.FastDeliveryId;
                }
                else if (inStockProducts.Any(x => x.ProductId == product.Id))
                {
                    conditionId = ClientFieldsConstants.Condition.IsStockId;
                }

                var completionDate = await _completionDatesRepository.GetAsync(
                    token,
                    language,
                    transportId,
                    conditionId,
                    zoneId,
                    campaignId,
                    DateTime.Now);

                product.CompletionDate = completionDate;
            }
        }

        public async Task GetCompletionDatesAsync(string token, string language, Product product, Guid? clientId)
        {
            var inStockProduct = _inventoryRepository.GetAvailbleProductsInventory(language, PaginationConstants.DefaultPageIndex, PaginationConstants.DefaultPageSize, token).Result.Data.FirstOrDefault(x => x.ProductId == product.Id);

            var clientFieldValues = await GetClientFieldValuesAsync(token, language, clientId);

            var transportId = GetParameterValue(clientFieldValues, ClientFieldsConstants.Transport.Id);
            var zoneId = GetParameterValue(clientFieldValues, ClientFieldsConstants.Zone.Id);
            var campaignId = GetParameterValue(clientFieldValues, ClientFieldsConstants.Campaign.Id);

            var conditionId = ClientFieldsConstants.Condition.StandardId;

            if (IsFastDeliveryEnable(product))
            {
                conditionId = ClientFieldsConstants.Condition.FastDeliveryId;
            }
            else if (inStockProduct is not null)
            {
                conditionId = ClientFieldsConstants.Condition.IsStockId;
            }

            var completionDate = await _completionDatesRepository.GetAsync(
                token,
                language,
                transportId,
                conditionId,
                zoneId,
                campaignId,
                DateTime.Now);

            product.CompletionDate = completionDate;

        }

        private Guid GetParameterValue(List<ClientFieldValue> clientFieldValues, Guid id)
        {
            if (clientFieldValues.Any(x => x.FieldDefinitionId == id))
            {
                var clientField = clientFieldValues.FirstOrDefault(y => y.FieldDefinitionId == id);

                if (clientField.FieldValue is not null)
                {
                    return Guid.Parse(clientField.FieldValue);
                }
            }

            return default;
        }

        private async Task<List<ClientFieldValue>> GetClientFieldValuesAsync(string token, string language, Guid? id)
        {
            var client = await _clientsRepository.GetClientAsync(token, language, id);

            return await _clientsRepository.GetClientFieldValuesAsync(token, language, client.Id);
        }

        private bool IsFastDeliveryEnable(Product product)
        {
            if (product.ProductAttributes.Any(x => x.Key == "fastDelivery") is false)
            {
                return false;
            }

            var fastDelivery = product.ProductAttributes.FirstOrDefault(x => x.Key == "fastDelivery");

            if (fastDelivery.Values.FirstOrDefault() == "true")
            {
                return true;
            }

            return false;
        }
    }
}
