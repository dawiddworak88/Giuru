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
        Task<Guid> CreateAsync(CreateClientFieldOptionServiceModel model);
        Task<Guid> UpdateAsync(UpdateClientFieldOptionServiceModel model);
        Task DeleteAsync(DeleteClientFieldOptionServiceModel model);
        Task<FieldOptionServiceModel> GetAsync(GetClientFieldOptionServiceModel model);
        PagedResults<IEnumerable<FieldOptionServiceModel>> Get(GetClientFieldOptionsServiceModel model);
    }
}
