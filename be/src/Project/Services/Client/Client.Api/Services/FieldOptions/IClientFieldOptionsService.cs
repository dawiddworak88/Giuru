using Client.Api.ServicesModels.FieldOptions;
using System;
using System.Threading.Tasks;

namespace Client.Api.Services.FieldOptions
{
    public interface IClientFieldOptionsService
    {
        Task<Guid> CreateAsync(CreateFieldOptionServiceModel model);
        Task<Guid> UpdateAsync(UpdateFieldOptionServiceModel model);
    }
}
