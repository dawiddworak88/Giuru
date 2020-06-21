using Foundation.Database.Areas.Tenants.Entities;
using Foundation.Database.Shared.Repositories;
using Foundation.Extensions.Definitions;
using Foundation.GenericRepository.Services;
using Foundation.Taxonomy.Models;
using Foundation.Taxonomy.ResultModels;
using Foundation.Taxonomy.Validators;
using Foundation.TenantDatabase.Areas.Translations.Entities;
using Foundation.TenantDatabase.Shared.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Foundation.Taxonomy.Services.TaxonomyServices
{
    public class TaxonomyService : ITaxonomyService
    {
        private readonly IGenericRepository<Tenant> tenantRepository;
        private readonly TenantGenericRepositoryFactory genericRepositoryFactory;
        private readonly IEntityService entityService;

        public TaxonomyService(
            IGenericRepository<Tenant> tenantRepository,
            TenantGenericRepositoryFactory genericRepositoryFactory,
            IEntityService entityService
            )
        {
            this.tenantRepository = tenantRepository;
            this.genericRepositoryFactory = genericRepositoryFactory;
            this.entityService = entityService;
        }

        public async Task<TaxonomyResultModel> CreateAsync(CreateTaxonomyModel model)
        {
            var validator = new CreateTaxonomyModelValidator();

            var validationResult = await validator.ValidateAsync(model);

            var createTaxonomyResultModel = new TaxonomyResultModel();

            if (!validationResult.IsValid)
            {
                createTaxonomyResultModel.Errors.AddRange(validationResult.Errors.Select(x => x.ErrorMessage));
                return createTaxonomyResultModel;
            }

            var tenant = this.tenantRepository.GetById(model.TenantId.Value);

            if (tenant == null)
            {
                createTaxonomyResultModel.Errors.Add(ErrorConstants.NoTenant);
                return createTaxonomyResultModel;
            }

            var taxonomy = new TenantDatabase.Areas.Taxonomies.Entities.Taxonomy
            {
                Name = model.Name,
                ParentId = model.ParentId
            };

            var context = await this.genericRepositoryFactory.CreateTenantDatabaseContext(tenant.DatabaseConnectionString);

            await context.Taxonomies.AddAsync(this.entityService.EnrichEntity(taxonomy, model.Username));

            await context.SaveChangesAsync();

            var translation = new Translation
            {
                Key = taxonomy.Id.ToString(),
                Value = taxonomy.Name,
                Language = model.Language
            };

            await context.Translations.AddAsync(this.entityService.EnrichEntity(translation, model.Username));

            await context.SaveChangesAsync();

            createTaxonomyResultModel.Taxonomy = taxonomy;

            return createTaxonomyResultModel;
        }

        public async Task<TaxonomyResultModel> GetByName(GetTaxonomyModel model)
        {
            var validator = new GetTaxonomyModelValidator();

            var validationResult = await validator.ValidateAsync(model);

            var getTaxonomyResultModel = new TaxonomyResultModel();

            if (!validationResult.IsValid)
            {
                getTaxonomyResultModel.Errors.AddRange(validationResult.Errors.Select(x => x.ErrorMessage));
                return getTaxonomyResultModel;
            }

            var tenant = this.tenantRepository.GetById(model.TenantId.Value);

            if (tenant == null)
            {
                getTaxonomyResultModel.Errors.Add(ErrorConstants.NoTenant);
                return getTaxonomyResultModel;
            }

            var context = await this.genericRepositoryFactory.CreateTenantDatabaseContext(tenant.DatabaseConnectionString);

            if (model.RootId.HasValue)
            {
                var taxonomies = context.Taxonomies.Where(x => x.Name == model.Name && x.IsActive);

                foreach (var taxonomy in taxonomies)
                {
                    Guid? rootId = taxonomy.ParentId;
                    var rootTaxonomy = taxonomy;

                    while (rootId.HasValue)
                    {
                        rootTaxonomy = context.Taxonomies.FirstOrDefault(x => x.Id == rootId.Value && x.IsActive);
                        rootId = rootTaxonomy.ParentId;
                    }

                    if (model.RootId == rootTaxonomy.Id)
                    {
                        return new TaxonomyResultModel
                        {
                            Taxonomy = taxonomy
                        };
                    }
                }
            }

            return new TaxonomyResultModel
            {
                Taxonomy = context.Taxonomies.FirstOrDefault(x => x.Name == model.Name && x.ParentId == null && x.IsActive)
            };
        }
    }
}

