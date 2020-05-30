using Foundation.Database.Areas.Tenants.Entities;
using Foundation.Database.Shared.Repositories;
using Foundation.Extensions.Definitions;
using Foundation.GenericRepository.Services;
using Foundation.Schema.Models;
using Foundation.Schema.ResultModels;
using Foundation.Schema.Validators;
using Foundation.TenantDatabase.Areas.Translations.Entities;
using Foundation.TenantDatabase.Shared.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace Foundation.Schema.Services.SchemaServices
{
    public class SchemaService : ISchemaService
    {
        private readonly IGenericRepository<Tenant> tenantRepository;
        private readonly TenantGenericRepositoryFactory genericRepositoryFactory;
        private readonly IEntityService entityService;

        public SchemaService(
            IGenericRepository<Tenant> tenantRepository,
            TenantGenericRepositoryFactory genericRepositoryFactory,
            IEntityService entityService
            )
        {
            this.tenantRepository = tenantRepository;
            this.genericRepositoryFactory = genericRepositoryFactory;
            this.entityService = entityService;
        }

        public async Task<SchemaResultModel> CreateAsync(CreateSchemaModel model)
        {
            var validator = new CreateSchemaModelValidator();

            var validationResult = await validator.ValidateAsync(model);

            var createSchemaResultModel = new SchemaResultModel();

            if (!validationResult.IsValid)
            {
                createSchemaResultModel.Errors.AddRange(validationResult.Errors.Select(x => x.ErrorMessage));
                return createSchemaResultModel;
            }

            var tenant = this.tenantRepository.GetById(model.TenantId.Value);

            if (tenant == null)
            {
                createSchemaResultModel.Errors.Add(ErrorConstants.NoTenant);
                return createSchemaResultModel;
            }

            var schema = new TenantDatabase.Areas.Schemas.Entities.Schema
            {
                Name = model.Name,
                EntityTypeId = model.EntityTypeId,
                JsonSchema = model.JsonSchema?.ToString(),
                UiSchema = model.UiSchema?.ToString()
            };

            var schemaRepository = await this.genericRepositoryFactory.CreateTenantGenericRepository<Foundation.TenantDatabase.Areas.Schemas.Entities.Schema>(tenant.DatabaseConnectionString);

            var translationRepository = await this.genericRepositoryFactory.CreateTenantGenericRepository<Translation>(tenant.DatabaseConnectionString);

            await schemaRepository.CreateAsync(this.entityService.EnrichEntity(schema, model.Username));

            await schemaRepository.SaveChangesAsync();

            var translation = new Translation
            {
                Key = schema.Id.ToString(),
                Value = schema.Name,
                Language = model.Language
            };

            await translationRepository.CreateAsync(this.entityService.EnrichEntity(translation, model.Username));

            await translationRepository.SaveChangesAsync();

            createSchemaResultModel.Schema = schema;

            return createSchemaResultModel;
        }
    }
}