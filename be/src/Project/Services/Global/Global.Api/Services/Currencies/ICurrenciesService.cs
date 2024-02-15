using Foundation.GenericRepository.Paginations;
using Global.Api.ServicesModels.Currencies;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Global.Api.Services.Currencies
{
    public interface ICurrenciesService
    {
        Task CreateAsync(CreateCurrencyServiceModel model);
        Task UpdateAsync(UpdateCurrencyServiceModel model);
        Task DeleteAsync(DeleteCurrencyServiceModel model);
        Task<CurrencyServiceModel> GetAsync(GetCurrencyServiceModel model);
        PagedResults<IEnumerable<CurrencyServiceModel>> Get(GetCurrenciesServiceModel model);    
    }
}