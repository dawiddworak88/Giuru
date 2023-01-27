using Analytics.Api.DomainModels;
using System;
using System.Threading.Tasks;

namespace Analytics.Api.Repositories.Global
{
    public interface ICountriesRepository
    {
        Task<Country> GetAsync(string token, string language, Guid? id);
    }
}
