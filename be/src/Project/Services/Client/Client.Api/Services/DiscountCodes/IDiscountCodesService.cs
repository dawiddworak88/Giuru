using Client.Api.ServicesModels.DiscountCodes;
using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Api.Services.DiscountCodes
{
    public interface IDiscountCodesService
    {
        Task<DiscountCodeServiceModel> CreateAsync(CreateDiscountCodeServiceModel model);
        Task<DiscountCodeServiceModel> UpdateAsync(UpdateDiscountCodeServiceModel model);
        Task DeleteAsync(DeleteDiscountCodeServiceModel model);
        Task<DiscountCodeServiceModel> GetAsync(GetDiscountCodeServiceModel model);
        Task<PagedResults<IEnumerable<DiscountCodeServiceModel>>> GetAsync(GetDiscountCodesServiceModel model);
        Task<PagedResults<IEnumerable<DiscountCodeClientOptionServiceModel>>> GetClientOptionsAsync(GetDiscountCodeClientOptionsServiceModel model);
        Task<DiscountCodeValidationServiceModel> ValidateAsync(ValidateDiscountCodeServiceModel model);
    }
}
