using Feature.Product.Models;
using Feature.Product.ResultModels;
using Feature.Product.Validators;
using Foundation.Database.Areas.Tenants.Entities;
using Foundation.Database.Shared.Repositories;
using Foundation.Extensions.Definitions;
using Foundation.GenericRepository.Paginations;
using Foundation.GenericRepository.Services;
using Foundation.Database.Areas.Translations.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;
using Foundation.Database.Shared.Contexts;

namespace Feature.Product.Services
{
    public class ProductService : IProductService
    {
        private readonly DatabaseContext context;
        private readonly IGenericRepository<Tenant> tenantRepository;
        private readonly IEntityService entityService;

        public ProductService(
            DatabaseContext context,
            IGenericRepository<Tenant> tenantRepository,
            IEntityService entityService)
        {
            this.context = context;
            this.tenantRepository = tenantRepository;
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

            var product = new Foundation.Database.Areas.Products.Entities.Product
            {
                Name = model.Name,
                Sku = model.Sku,
                SchemaId = model.SchemaId,
                FormData = model.FormData
            };

            await this.context.Products.AddAsync(this.entityService.EnrichEntity(product, model.Username));

            await this.context.SaveChangesAsync();

            if (!string.IsNullOrWhiteSpace(product.Name))
            {
                var translation = new Translation
                {
                    Key = product.Id.ToString(),
                    Value = product.Name,
                    Language = model.Language
                };

                await this.context.Translations.AddAsync(this.entityService.EnrichEntity(translation, model.Username));

                await this.context.SaveChangesAsync();
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

            var product = this.context.Products.FirstOrDefault(x => x.Id == model.Id.Value && x.IsActive);

            product.Name = model.Name;
            product.Sku = model.Sku;
            product.SchemaId = model.SchemaId;
            product.FormData = model.FormData;
            product.LastModifiedDate = DateTime.UtcNow;

            await this.context.SaveChangesAsync();

            var translation = this.context.Translations.FirstOrDefault(x => x.Key == product.Id.ToString() && x.Language == model.Language && x.IsActive);

            if (translation != null)
            {
                translation.Value = model.Name;
                translation.LastModifiedDate = DateTime.UtcNow;

                await this.context.SaveChangesAsync();
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

            var translations = this.context.Translations.Where(x => x.Key == deleteProductModel.Id.ToString() && x.IsActive).ToList();

            foreach (var translation in translations)
            {
                translation.IsActive = false;

                await this.context.SaveChangesAsync();
            }

            var product = this.context.Products.FirstOrDefault(x => x.Id == deleteProductModel.Id.Value && x.IsActive);

            if (product != null)
            {
                product.IsActive = false;

                await this.context.SaveChangesAsync();
            }

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

            var entities = this.context.Products.Where(x => x.IsActive);

            if (!string.IsNullOrWhiteSpace(getProductsModel.SearchTerm))
            {
                entities = entities.Where(x => (!string.IsNullOrWhiteSpace(x.Sku) && x.Sku.StartsWith(getProductsModel.SearchTerm)) || (!string.IsNullOrWhiteSpace(x.Name) && x.Name.Contains(getProductsModel.SearchTerm)));
            }

            entities = entities.OrderByDescending(x => x.CreatedDate);            

            var pagination = new Pagination(entities.Count(), getProductsModel.ItemsPerPage);

            getProductsResultModel.Products = entities.PagedIndex(pagination, getProductsModel.PageIndex);

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

            var product = this.context.Products.FirstOrDefault(x => x.Id == getProductModel.Id.Value && x.IsActive);

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
