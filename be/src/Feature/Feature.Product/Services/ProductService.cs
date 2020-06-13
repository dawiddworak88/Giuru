using Feature.Product.Models;
using Feature.Product.ResultModels;
using Feature.Product.Validators;
using Foundation.Database.Areas.Tenants.Entities;
using Foundation.Database.Shared.Repositories;
using Foundation.Extensions.Definitions;
using Foundation.GenericRepository.Predicates;
using Foundation.GenericRepository.Services;
using Foundation.TenantDatabase.Areas.Translations.Entities;
using Foundation.TenantDatabase.Shared.Repositories;
using System;
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

        public async Task<ProductResultModel> CreateAsync(CreateUpdateProductModel model)
        {
            var validator = new CreateUpdateProductModelValidator();

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
                Sku = model.Sku,
                FormData = model.FormData
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

        public async Task<ProductResultModel> UpdateAsync(CreateUpdateProductModel model)
        {
            var validator = new CreateUpdateProductModelValidator();

            var validationResult = await validator.ValidateAsync(model);

            var productResultModel = new ProductResultModel();

            if (!validationResult.IsValid)
            {
                productResultModel.Errors.AddRange(validationResult.Errors.Select(x => x.ErrorMessage));
                return productResultModel;
            }

            var tenant = this.tenantRepository.GetById(model.TenantId.Value);

            if (tenant == null)
            {
                productResultModel.Errors.Add(ErrorConstants.NoTenant);
                return productResultModel;
            }

            var productRepository = await this.genericRepositoryFactory.CreateTenantGenericRepository<Foundation.TenantDatabase.Areas.Products.Entities.Product>(tenant.DatabaseConnectionString);

            var product = productRepository.GetById(model.Id.Value);

            product.Name = model.Name;
            product.Sku = model.Sku;
            product.FormData = model.FormData;
            product.LastModifiedDate = DateTime.UtcNow;

            await productRepository.SaveChangesAsync();

            var translationRepository = await this.genericRepositoryFactory.CreateTenantGenericRepository<Foundation.TenantDatabase.Areas.Translations.Entities.Translation>(tenant.DatabaseConnectionString);

            var translation = translationRepository.Get(x => x.Key == product.Id.ToString() && x.Language == model.Language && x.IsActive).FirstOrDefault();

            if (translation != null)
            {
                translation.Value = model.Name;
                translation.LastModifiedDate = DateTime.UtcNow;

                await translationRepository.SaveChangesAsync();
            }

            productResultModel.Product = product;

            return productResultModel;
        }

        public async Task<DeleteProductResultModel> DeleteAsync(DeleteProductModel deleteProductModel)
        {
            var validator = new DeleteProductModelValidator();

            var validationResult = await validator.ValidateAsync(deleteProductModel);

            var deleteProductResultModel = new DeleteProductResultModel();

            if (!validationResult.IsValid)
            {
                deleteProductResultModel.Errors.AddRange(validationResult.Errors.Select(x => x.ErrorMessage));
                return deleteProductResultModel;
            }

            var tenant = this.tenantRepository.GetById(deleteProductModel.TenantId.Value);

            if (tenant == null)
            {
                deleteProductResultModel.Errors.Add(ErrorConstants.NoTenant);
                return deleteProductResultModel;
            }

            var translationRepository = await this.genericRepositoryFactory.CreateTenantGenericRepository<Foundation.TenantDatabase.Areas.Translations.Entities.Translation>(tenant.DatabaseConnectionString);

            var translations = translationRepository.Get(x => x.Key == deleteProductModel.Id.ToString() && x.IsActive).ToList();

            foreach (var translation in translations)
            {
                translationRepository.Delete(translation.Id);

                await translationRepository.SaveChangesAsync();
            }

            var productRepository = await this.genericRepositoryFactory.CreateTenantGenericRepository<Foundation.TenantDatabase.Areas.Products.Entities.Product>(tenant.DatabaseConnectionString);

            productRepository.Delete(deleteProductModel.Id.Value);

            await productRepository.SaveChangesAsync();

            return deleteProductResultModel;
        }

        public async Task<ProductsResultModel> GetAsync(GetProductsModel getProductsModel)
        {
            var validator = new GetProductsModelValidator();

            var validationResult = await validator.ValidateAsync(getProductsModel);

            var getProductsResultModel = new ProductsResultModel();

            if (!validationResult.IsValid)
            {
                getProductsResultModel.Errors.AddRange(validationResult.Errors.Select(x => x.ErrorMessage));
                return getProductsResultModel;
            }

            var tenant = this.tenantRepository.GetById(getProductsModel.TenantId.Value);

            if (tenant == null)
            {
                getProductsResultModel.Errors.Add(ErrorConstants.NoTenant);
                return getProductsResultModel;
            }

            var productRepository = await this.genericRepositoryFactory.CreateTenantGenericRepository<Foundation.TenantDatabase.Areas.Products.Entities.Product>(tenant.DatabaseConnectionString);

            var predicate = PredicateBuilder.True<Foundation.TenantDatabase.Areas.Products.Entities.Product>();

            if (!string.IsNullOrWhiteSpace(getProductsModel.SearchTerm))
            {
                predicate = predicate.And(x => x.Sku.StartsWith(getProductsModel.SearchTerm) || x.Name.Contains(getProductsModel.SearchTerm));
            }

            predicate = predicate.And(x => x.IsActive);

            var products = productRepository.GetPaged(getProductsModel.PageIndex, getProductsModel.ItemsPerPage, predicate.Compile(), x=> x.CreatedDate, true);

            getProductsResultModel.Products = products;

            return getProductsResultModel;
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
