using Buyer.Web.Shared.DomainModels.Global;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Repositories.Global
{
    public interface IGlobalRepository
    {
        Task<IEnumerable<Country>> GetCountriesAsync(string token, string language, string orderBy);
    }
}
