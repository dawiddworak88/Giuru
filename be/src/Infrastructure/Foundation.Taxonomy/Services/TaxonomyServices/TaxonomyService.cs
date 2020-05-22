using Foundation.Database.Areas.Tenants.Entities;
using Foundation.Database.Shared.Repositories;
using Foundation.Extensions.Definitions;
using Foundation.GenericRepository.Services;
using Foundation.Taxonomy.Models;
using Foundation.Taxonomy.ResultModels;
using Foundation.Taxonomy.Validators;
using Foundation.TenantDatabase.Areas.Translations.Entities;
using Foundation.TenantDatabase.Shared.Repositories;
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

            var taxonomyRepository = await this.genericRepositoryFactory.CreateTenantGenericRepository<Foundation.TenantDatabase.Areas.Taxonomies.Entities.Taxonomy>(tenant.DatabaseConnectionString);

            var translationRepository = await this.genericRepositoryFactory.CreateTenantGenericRepository<Translation>(tenant.DatabaseConnectionString);

            await taxonomyRepository.CreateAsync(this.entityService.EnrichEntity(taxonomy, model.Username));

            await taxonomyRepository.SaveChangesAsync();

            var translation = new Translation
            {
                Key = taxonomy.Id.ToString(),
                Value = taxonomy.Name,
                Language = model.Language
            };

            await translationRepository.CreateAsync(this.entityService.EnrichEntity(translation, model.Username));

            await translationRepository.SaveChangesAsync();

            createTaxonomyResultModel.Taxonomy = taxonomy;

            return createTaxonomyResultModel;
        }
    }
}
