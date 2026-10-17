using Client.Api.ServicesModels.DiscountCodes;
using Foundation.GenericRepository.Paginations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Api.Services.DiscountCodes
{
    public interface IDiscountCodesService
    {
        Task<PagedResults<IEnumerable<DiscountCodeServiceModel>>> GetAsync(GetDiscountCodesServiceModel model);
        Task<DiscountCodeServiceModel> GetAsync(GetDiscountCodeServiceModel model);
        Task<Guid> SaveAsync(SaveDiscountCodeServiceModel model);
        Task DeleteAsync(DeleteDiscountCodeServiceModel model);
    }
}
