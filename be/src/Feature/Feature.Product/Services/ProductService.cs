using Feature.Product.Models;
using Feature.Product.Validators;
using Foundation.Database.Areas.Tenants.Entities;
using Foundation.Database.Shared.Repositories;
using Foundation.TenantDatabase.Shared.Contexts;
using Foundation.TenantDatabase.Shared.Repositories;
using System.Threading.Tasks;

namespace Feature.Product.Services
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Tenant> tenantRepository;
        private readonly TenantGenericRepositoryFactory genericRepositoryFactory;
        private readonly TenantDatabaseContextFactory tenantDatabaseContextFactory;

        public ProductService(
            IGenericRepository<Tenant> tenantRepository,
            TenantGenericRepositoryFactory genericRepositoryFactory,
            TenantDatabaseContextFactory tenantDatabaseContextFactory
            )
        {
            this.tenantRepository = tenantRepository;
            this.genericRepositoryFactory = genericRepositoryFactory;
            this.tenantDatabaseContextFactory = tenantDatabaseContextFactory;
        }

        public async Task<CreateProductResultModel> CreateAsync(CreateProductModel model)
        {
            var validator = new CreateProductModelValidator();

            var validationResult = await validator.ValidateAsync(model);

            var createClientResultModel = new CreateProductResultModel
            {
                ValidationResult = validationResult
            };

            if (validationResult.IsValid)
            {
                var tenant = this.tenantRepository.GetById(model.TenantId);

                if (tenant != null)
                {
                    var productRepository = await this.genericRepositoryFactory.CreateTenantGenericRepository<Foundation.TenantDatabase.Areas.Products.Entities.Product>(tenant.DatabaseConnectionString);
                    
                    var product = new Foundation.TenantDatabase.Areas.Products.Entities.Product
                    { 
                        Version = 1
                    };

                    await productRepository.CreateAsync(product);

                    createClientResultModel.Product = product;
                }
            }

            return createClientResultModel;
        }
    }
}
