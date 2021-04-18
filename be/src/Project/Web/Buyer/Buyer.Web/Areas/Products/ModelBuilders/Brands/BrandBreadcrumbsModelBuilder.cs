using Buyer.Web.Areas.Products.ViewModels.Brands;
using Buyer.Web.Shared.Repositories.Brands;
using Buyer.Web.Shared.ModelBuilders.Breadcrumbs;
using Buyer.Web.Shared.ViewModels.Breadcrumbs;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using System.Globalization;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.ModelBuilders.Brands
{
    public class BrandBreadcrumbsModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, BrandBreadcrumbsViewModel>
    {
        private readonly LinkGenerator linkGenerator;
        private readonly IBrandRepository brandRepository;
        private readonly IBreadcrumbsModelBuilder<ComponentModelBase, BrandBreadcrumbsViewModel> breadcrumbsModelBuilder;

        public BrandBreadcrumbsModelBuilder(
            LinkGenerator linkGenerator,
            IBrandRepository brandRepository,
            IBreadcrumbsModelBuilder<ComponentModelBase, BrandBreadcrumbsViewModel> breadcrumbsModelBuilder)
        {
            this.linkGenerator = linkGenerator;
            this.breadcrumbsModelBuilder = breadcrumbsModelBuilder;
            this.brandRepository = brandRepository;
        }

        public async Task<BrandBreadcrumbsViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = this.breadcrumbsModelBuilder.BuildModel(componentModel);

            var brand = await this.brandRepository.GetBrandAsync(componentModel.Id, componentModel.Token, componentModel.Language);

            if (brand != null)
            {
                var brandBreadcrumb = new BreadcrumbViewModel
                {
                    Name = brand.Name,
                    Url = this.linkGenerator.GetPathByAction("Index", "Brand", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, Id = brand.Id }),
                    IsActive = true
                };

                viewModel.Items.Add(brandBreadcrumb);
            }

            return viewModel;
        }
    }
}
