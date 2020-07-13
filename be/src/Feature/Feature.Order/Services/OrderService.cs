using Feature.Order.Models;
using Feature.Order.ResultModels;
using Foundation.Database.Areas.Tenants.Entities;
using Foundation.Database.Shared.Contexts;
using Foundation.Database.Shared.Repositories;
using Foundation.Extensions.Definitions;
using Foundation.GenericRepository.Services;
using Microsoft.Extensions.Localization;
using System.Linq;
using System.Threading.Tasks;

namespace Feature.Order.Services
{
    public class OrderService : IOrderService
    {
        private readonly DatabaseContext context;
        private readonly IStringLocalizer<OrderResources> orderLocalizer;
        private readonly IGenericRepository<Tenant> tenantRepository;
        private readonly IEntityService entityService;

        public OrderService(
            DatabaseContext context,
            IStringLocalizer<OrderResources> orderLocalizer,
            IGenericRepository<Tenant> tenantRepository,
            IEntityService entityService)
        {
            this.context = context;
            this.orderLocalizer = orderLocalizer;
            this.tenantRepository = tenantRepository;
            this.entityService = entityService;
        }

        public async Task<OrderValidationResultModel> ValidateOrderAsync(OrderValidationModel model)
        {
            var orderValidationResultModel = new OrderValidationResultModel();

            var tenant = this.tenantRepository.GetById(model.TenantId.Value);

            if (tenant == null)
            {
                orderValidationResultModel.Errors.Add(ErrorConstants.NoTenant);
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