using Inventory.Api.Infrastructure;
using Inventory.Api.ServicesModels.OutletServices;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace Inventory.Api.Services.Outlet
{
    public class OutletService : IOutletService
    {
        private readonly InventoryContext context;
        private readonly IStringLocalizer inventortLocalizer;

        public OutletService(
            InventoryContext context,
            IStringLocalizer inventortLocalizer)
        {
            this.context = context;
            this.inventortLocalizer = inventortLocalizer;
        }

        public Task<OutletServiceModel> SaveOutletAsync(OutletServiceModel model)
        {
            throw new System.NotImplementedException();
        }
    }
}
