using Feature.Order.Models;
using Feature.Order.ResultModels;
using Foundation.Database.Areas.Tenants.Entities;
using Foundation.Database.Shared.Repositories;
using Foundation.GenericRepository.Services;
using Foundation.TenantDatabase.Shared.Repositories;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace Feature.Order.Services
{
    public class OrderService : IOrderService
    {
        private readonly IStringLocalizer<OrderResources> orderLocalizer;
        private readonly IGenericRepository<Tenant> tenantRepository;
        private readonly TenantGenericRepositoryFactory genericRepositoryFactory;
        private readonly IEntityService entityService;

        public OrderService(
            IStringLocalizer<OrderResources> orderLocalizer,
            IGenericRepository<Tenant> tenantRepository,
            TenantGenericRepositoryFactory genericRepositoryFactory,
            IEntityService entityService)
        {
            this.orderLocalizer = orderLocalizer;
            this.tenantRepository = tenantRepository;
            this.genericRepositoryFactory = genericRepositoryFactory;
            this.entityService = entityService;
        }

        public Task<OrderValidationResultModel> ValidateOrderAsync(OrderValidationModel model)
        {
            var orderValidationResultModel = new OrderValidationResultModel();

            var tenant = this.tenantRepository.GetById(model.TenantId.Value);

            if (tenant == null)
            {
                orderValidationResultModel.Errors.Add(ErrorConstants.NoTenant);
                return orderValidationResultModel;
            }
        }
    }
}
