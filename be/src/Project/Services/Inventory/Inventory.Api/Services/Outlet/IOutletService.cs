using Inventory.Api.ServicesModels.OutletServices;
using System.Threading.Tasks;

namespace Inventory.Api.Services.Outlet
{
    public interface IOutletService
    {
        Task<OutletServiceModel> SaveOutletAsync(OutletServiceModel model);
    }
}
