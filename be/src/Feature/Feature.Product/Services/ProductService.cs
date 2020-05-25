using Feature.Product.Models;
using Feature.Product.ResultModels;
using Feature.Product.Validators;
using Foundation.Database.Areas.Tenants.Entities;
using Foundation.Database.Shared.Repositories;
using Foundation.Extensions.Definitions;
using Foundation.GenericRepository.Services;
using Foundation.TenantDatabase.Areas.Translations.Entities;
using Foundation.TenantDatabase.Shared.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace Feature.Product.Services
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Tenant> tenantRepository;
        private readonly TenantGenericRepositoryFactory genericRepositoryFactory;
        private readonly IEntityService entityService;

        public ProductService(
            IGenericRepository<Tenant> tenantRepository,
            TenantGenericRepositoryFactory genericRepositoryFactory,
            IEntityService entityService)
        {
            this.tenantRepository = tenantRepository;
            this.genericRepositoryFactory = genericRepositoryFactory;
            this.entityService = entityService;
        }

        public async Task<ProductResultModel> CreateAsync(CreateProductModel model)
        {
            var validator = new CreateProductModelValidator();

            var validationResult = await validator.ValidateAsync(model);

            var createProductResultModel = new ProductResultModel();

            if (!validationResult.IsValid)
            {
                createProductResultModel.Errors.AddRange(validationResult.Errors.Select(x => x.ErrorMessage));
                return createProductResultModel;
            }

            var tenant = this.tenantRepository.GetById(model.TenantId.Value);

            if (tenant == null)
            {
                createProductResultModel.Errors.Add(ErrorConstants.NoTenant);
                return createProductResultModel;
            }

            var product = new Foundation.TenantDatabase.Areas.Products.Entities.Product
            {
                Name = model.Name,
                Sku = model.Sku
            };

            var productRepository = await this.genericRepositoryFactory.CreateTenantGenericRepository<Foundation.TenantDatabase.Areas.Products.Entities.Product>(tenant.DatabaseConnectionString);
            
            var translationRepository = await this.genericRepositoryFactory.CreateTenantGenericRepository<Translation>(tenant.DatabaseConnectionString);

            await productRepository.CreateAsync(this.entityService.EnrichEntity(product, model.Username));

            await productRepository.SaveChangesAsync();

            if (!string.IsNullOrWhiteSpace(product.Name))
            {
                var translation = new Translation
                {
                    Key = product.Id.ToString(),
                    Value = product.Name,
                    Language = model.Language
                };

                await translationRepository.CreateAsync(this.entityService.EnrichEntity(translation, model.Username));

                await translationRepository.SaveChangesAsync();
            }

            createProductResultModel.Product = product;

            return createProductResultModel;
        }

        public async Task<ProductResultModel> GetByIdAsync(GetProductModel getProductModel)
        {
            var validator = new GetProductModelValidator();

            var validationResult = await validator.ValidateAsync(getProductModel);

            var getProductResultModel = new ProductResultModel();

            if (!validationResult.IsValid)
            {
                getProductResultModel.Errors.AddRange(validationResult.Errors.Select(x => x.ErrorMessage));
                return getProductResultModel;
            }

            var tenant = this.tenantRepository.GetById(getProductModel.TenantId.Value);

            if (tenant == null)
            {
                getProductResultModel.Errors.Add(ErrorConstants.NoTenant);
                return getProductResultModel;
            }

            var productRepository = await this.genericRepositoryFactory.CreateTenantGenericRepository<Foundation.TenantDatabase.Areas.Products.Entities.Product>(tenant.DatabaseConnectionString);

            var product = productRepository.GetById(getProductModel.Id.Value);

            if (product == null)
            {
                getProductResultModel.Errors.Add(ErrorConstants.NotFound);
                return getProductResultModel;
            }

            getProductResultModel.Product = product;

            return getProductResultModel;
        }
    }
}
