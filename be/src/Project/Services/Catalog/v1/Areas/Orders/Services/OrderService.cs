using Catalog.Api.v1.Areas.Schemas.Models;
using Catalog.Api.v1.Areas.Schemas.ResultModels;
using Foundation.Database.Areas.Sellers.Entities;
using Foundation.Database.Shared.Contexts;
using Foundation.Database.Shared.Repositories;
using Foundation.Extensions.Definitions;
using Foundation.GenericRepository.Services;
using Foundation.Localization;
using Microsoft.Extensions.Localization;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Api.v1.Areas.Schemas.Services
{
    public class OrderService : IOrderService
    {
        private readonly DatabaseContext context;
        private readonly IStringLocalizer<OrderResources> orderLocalizer;
        private readonly IGenericRepository<Seller> sellerRepository;
        private readonly IEntityService entityService;

        public OrderService(
            DatabaseContext context,
            IStringLocalizer<OrderResources> orderLocalizer,
            IGenericRepository<Seller> sellerRepository,
            IEntityService entityService)
        {
            this.context = context;
            this.orderLocalizer = orderLocalizer;
            this.sellerRepository = sellerRepository;
            this.entityService = entityService;
        }

        public async Task<OrderValidationResultModel> ValidateOrderAsync(OrderValidationModel model)
        {
            var orderValidationResultModel = new OrderValidationResultModel();

            var seller = this.sellerRepository.GetById(model.SellerId.Value);

            if (seller == null)
            {
                orderValidationResultModel.Errors.Add(ErrorConstants.NoSeller);
                return orderValidationResultModel;
            }

            if (!model.ClientId.HasValue)
            {
                orderValidationResultModel.ValidationMessages.Add(this.orderLocalizer["NoClientId"]);
            }
            else
            {
                var client = this.context.Clients.FirstOrDefault(x => x.Id == model.ClientId && x.IsActive);

                if (client == null)
                {
                    orderValidationResultModel.ValidationMessages.Add(this.orderLocalizer["NoClient"]);
                }
            }

            if (model.OrderItems == null || !model.OrderItems.Any())
            {
                orderValidationResultModel.ValidationMessages.Add(this.orderLocalizer["NoOrderItems"]);
            }
            else
            {
                int orderItemIndex = 1;

                foreach (var orderItem in model.OrderItems)
                {
                    if (string.IsNullOrWhiteSpace(orderItem.Sku))
                    {
                        orderValidationResultModel.ValidationMessages.Add($"{this.orderLocalizer["EmptySkuForOrderItemIndex"]}: {orderItemIndex}");
                    }
                    else
                    {
                        var product = this.context.Products.FirstOrDefault(x => x.Sku == orderItem.Sku && x.IsActive);

                        if (product == null)
                        {
                            orderValidationResultModel.ValidationMessages.Add($"{this.orderLocalizer["NoProductBySku"]}: {orderItem.Sku}");
                        }
                    }

                    if (!orderItem.Quantity.HasValue || orderItem.Quantity < 1)
                    {
                        if (!string.IsNullOrWhiteSpace(orderItem.Sku))
                        {
                            orderValidationResultModel.ValidationMessages.Add($"{this.orderLocalizer["IncorrectQuantityForSku"]}: {orderItemIndex}");
                        }
                        else
                        {
                            orderValidationResultModel.ValidationMessages.Add($"{this.orderLocalizer["IncorrectQuantityForOrderItemIndex"]}: {orderItemIndex}");
                        }
                    }

                    orderItemIndex++;
                }
            }

            return orderValidationResultModel;
        }
    }
}