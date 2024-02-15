using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;
using System.Threading.Tasks;
using Global.Api.ServicesModels.Countries;

namespace Global.Api.Services.Countries
{
    public interface ICountriesService
    {
        PagedResults<IEnumerable<CountryServiceModel>> Get(GetCountriesServiceModel model);
        Task CreateAsync(CreateCountryServiceModel model);
        Task UpdateAsync(UpdateCountryServiceModel model);
        Task<CountryServiceModel> GetAsync(GetCountryServiceModel model);
        Task DeleteAsync(DeleteCountryServiceModel model);
    }
}