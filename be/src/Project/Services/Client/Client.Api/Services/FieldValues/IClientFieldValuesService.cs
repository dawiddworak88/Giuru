using Client.Api.ServicesModels.Fields;
using Client.Api.ServicesModels.FieldValues;
using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Api.Services.FieldValues
{
    public interface IClientFieldValuesService
    {
        Task CreateAsync(CreateClientFieldValuesServiceModel model);
        PagedResults<IEnumerable<ClientFieldValueServiceModel>> Get(GetClientFieldValuesServiceModel model);
    }
}
