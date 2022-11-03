using Foundation.Extensions.ModelBuilders;
using Microsoft.Extensions.DependencyInjection;
using Seller.Web.Areas.ModelBuilders.Products;
using Seller.Web.Areas.Products.Repositories;
using Seller.Web.Areas.Products.ViewModels;
using Foundation.PageContent.ComponentModels;
using Seller.Web.Areas.Categories.ModelBuilders;
using Seller.Web.Areas.Products.DomainModels;
using Seller.Web.Shared.ViewModels;
using Seller.Web.Areas.Categories.Repositories;
using Seller.Web.Areas.ProductAttributes.Repositories;
using Seller.Web.Areas.ProductAttributeItems.ModelBuilders;
using Seller.Web.Areas.Products.ComponentModels;
using Seller.Web.Areas.Products.ModelBuilders;

namespace Seller.Web.Areas.Products.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterProductsAreaDependencies(this IServiceCollection services)
        {
            services.AddScoped<IProductAttributesRepository, ProductAttributesRepository>();
            services.AddScoped<IProductAttributeItemsRepository, ProductAttributeItemsRepository>();
            services.AddScoped<ICategoriesRepository, CategoriesRepository>();

            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CategoriesPageViewModel>, CategoriesPageModelBuilder> ();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CategoryPageViewModel>, CategoryPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CategoryFormViewModel>, CategoryFormModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<Category>>, CategoriesPageCatalogModelBuilder>();

            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ProductsPageViewModel>, ProductsPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ProductAttributesPageViewModel>, ProductAttributesPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ProductAttributePageViewModel>, ProductAttributePageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ProductAttributeFormViewModel>, ProductAttributeFormModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ProductAttributeItemComponentModel, ProductAttributeItemPageViewModel>, ProductAttributeItemPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ProductAttributeItemComponentModel, ProductAttributeItemFormViewModel>, ProductAttributeItemFormModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<ProductAttribute>>, ProductAttributesPageCatalogModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<ProductAttributeItem>>, ProductAttributePageCatalogModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ProductFormViewModel>, ProductFormModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ProductPageViewModel>, ProductPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<DuplicateProductComponentModel, ProductPageViewModel>, DuplicateProductPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<DuplicateProductComponentModel, ProductFormViewModel>, DuplicateProductFormModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<DuplicateCategoryComponentModel, CategoryPageViewModel>, DuplicateCategoryPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<DuplicateCategoryComponentModel, CategoryFormViewModel>, DuplicateCategoryFormModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<Product>>, ProductsPageCatalogModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CategoryBaseFormViewModel>, CategoryBaseFormModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ProductBaseFormViewModel>, ProductBaseFormModelBuilder>();
        }
    }
}
