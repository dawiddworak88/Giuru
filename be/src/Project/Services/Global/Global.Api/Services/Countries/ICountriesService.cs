using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Global.Api.ServicesModels.Countries;

namespace Global.Api.Services.Countries
{
    public interface ICountriesService
    {
        Task<PagedResults<IEnumerable<CountryServiceModel>>> GetAsync(GetCountriesServiceModel model);
        Task CreateAsync(CreateCountryServiceModel model);
        Task UpdateAsync(UpdateCountryServiceModel model);
        Task<CountryServiceModel> GetAsync(GetCountryServiceModel model);
        Task DeleteAsync(DeleteCountryServiceModel model);
    }
}
