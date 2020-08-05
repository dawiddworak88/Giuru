using Catalog.Api.v1.Areas.Taxonomies.Models;
using Catalog.Api.v1.Areas.Taxonomies.ResultModels;
using System.Threading.Tasks;

namespace Catalog.Api.v1.Areas.Taxonomies.Services.TaxonomyServices
{
    public interface ITaxonomyService
    {
        Task<TaxonomyResultModel> CreateAsync(CreateTaxonomyModel model);
        Task<TaxonomyResultModel> GetByName(GetTaxonomyModel model);
    }
}
