using Client.Api.ServicesModels.DiscountCodes;
using Foundation.GenericRepository.Paginations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Api.Services.DiscountCodes
{
    public interface IDiscountCodesService
    {
        Task<Guid> CreateAsync(CreateDiscountCodeServiceModel model);
        Task<Guid> UpdateAsync(UpdateDiscountCodeServiceModel model);
        PagedResults<IEnumerable<DiscountCodeServiceModel>> Get(GetDiscountCodesServiceModel model);
        Task<DiscountCodeServiceModel> GetAsync(GetDiscountCodeServiceModel model);
        Task DeleteAsync(DeleteDiscountCodeServiceModel model);

        /// <summary>
        /// Answers whether a code can be applied for a client. "Not applicable" is an answer, not an error.
        /// </summary>
        Task<DiscountCodeValidationServiceModel> ValidateAsync(ValidateDiscountCodeServiceModel model);
    }
}
