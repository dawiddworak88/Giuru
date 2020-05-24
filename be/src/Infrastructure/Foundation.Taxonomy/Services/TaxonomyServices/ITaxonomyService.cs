using Foundation.Taxonomy.Models;
using Foundation.Taxonomy.ResultModels;
using System.Threading.Tasks;

namespace Foundation.Taxonomy.Services.TaxonomyServices
{
    public interface ITaxonomyService
    {
        Task<TaxonomyResultModel> CreateAsync(CreateTaxonomyModel model);
        Task<TaxonomyResultModel> GetByName(GetTaxonomyModel model);
    }
}
