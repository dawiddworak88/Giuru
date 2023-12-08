using Client.Api.ServicesModels.FieldOptions;
using Client.Api.ServicesModels.Fields;
using Foundation.GenericRepository.Paginations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Api.Services.FieldOptions
{
    public interface IClientFieldOptionsService
    {
        Task<Guid> CreateAsync(CreateFieldOptionServiceModel model);
        Task<Guid> UpdateAsync(UpdateFieldOptionServiceModel model);
        Task DeleteAsync(DeleteFieldOptionServiceModel model);
        Task<FieldOptionServiceModel> GetAsync(GetFieldOptionServiceModel model);
        PagedResults<IEnumerable<FieldOptionServiceModel>> Get(GetFieldOptionsServiceModel model);
    }
}
