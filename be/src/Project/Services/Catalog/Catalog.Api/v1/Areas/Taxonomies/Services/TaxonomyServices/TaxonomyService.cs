using Foundation.Extensions.Definitions;
using Foundation.GenericRepository.Services;
using Catalog.Api.v1.Areas.Taxonomies.Models;
using Catalog.Api.v1.Areas.Taxonomies.ResultModels;
using Catalog.Api.v1.Areas.Taxonomies.Validators;
using System;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Api.Infrastructure.Taxonomies.Entities;
using Catalog.Api.Infrastructure;

namespace Catalog.Api.v1.Areas.Taxonomies.Services.TaxonomyServices
{
    public class TaxonomyService : ITaxonomyService
    {
        private readonly CatalogContext context;
        private readonly IEntityService entityService;

        public TaxonomyService(
            CatalogContext context,
            IEntityService entityService
            )
        {
            this.context = context;
            this.entityService = entityService;
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

