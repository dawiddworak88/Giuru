using Catalog.Api.v1.Areas.Taxonomies.Models;
using Catalog.Api.v1.Areas.Taxonomies.ResultModels;
using System.Threading.Tasks;
using Catalog.Api.Infrastructure;

namespace Catalog.Api.v1.Areas.Taxonomies.Services.TaxonomyServices
{
    public class TaxonomyService : ITaxonomyService
    {
        private readonly CatalogContext context;

        public TaxonomyService(
            CatalogContext context)
        {
            this.context = context;
        }

        public async Task<TaxonomyResultModel> CreateAsync(CreateTaxonomyModel model)
        {
            return default;
        }

        public async Task<TaxonomyResultModel> GetByName(GetTaxonomyModel model)
        {
            return default;
        }
    }
}

