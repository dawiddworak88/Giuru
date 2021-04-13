using Buyer.Web.Areas.Home.Definitions;
using Buyer.Web.Areas.Home.ViewModel;
using Buyer.Web.Shared.Services.Catalogs;
using Buyer.Web.Shared.Configurations;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.Extensions.Services.MediaServices;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.ContentGrids.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Localization;
using Foundation.Localization;

namespace Buyer.Web.Areas.Home.ModelBuilders
{
    public class HomePageContentGridModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, HomePageContentGridViewModel>
    {
        private readonly ICatalogService catalogService;
        private readonly IOptions<AppSettings> options;
        private readonly IMediaHelperService mediaService;
        private readonly LinkGenerator linkGenerator;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;

        public HomePageContentGridModelBuilder(
            ICatalogService catalogService, 
            IOptions<AppSettings> options, 
            IMediaHelperService mediaService,
            LinkGenerator linkGenerator,
            IStringLocalizer<GlobalResources> globalLocalizer)
        {
            this.catalogService = catalogService;
            this.options = options;
            this.mediaService = mediaService;
            this.linkGenerator = linkGenerator;
            this.globalLocalizer = globalLocalizer;
        }

        public async Task<HomePageContentGridViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var items = new List<ContentGridItemViewModel>();

            var newProducts = await this.catalogService.GetCatalogProductsAsync(
                componentModel.Token,
                componentModel.Language,
                null,
                false,
                true,
                null,
                Foundation.GenericRepository.Definitions.Constants.DefaultPageIndex,
                Foundation.GenericRepository.Definitions.Constants.DefaultItemsPerPage);

            if (newProducts != null && newProducts.Total > 0)
            {
                var contentGridCarouselItems = new List<ContentGridCarouselItemViewModel>();

                foreach (var newProduct in newProducts.Data)
                {
                    var carouselItem = new ContentGridCarouselItemViewModel
                    {
                        Id = newProduct.Id,
                        Title = newProduct.Title,
                        ImageAlt = newProduct.ImageAlt,
                        ImageUrl = newProduct.ImageUrl,
                        Url = this.linkGenerator.GetPathByAction("Index", "Product", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, newProduct.Id })
                    };

                    contentGridCarouselItems.Add(carouselItem);
                }

                items.Add(new ContentGridItemViewModel { Id = ContentGridConstants.Novelties.NoveltiesId, Title = this.globalLocalizer.GetString("Novelties"), CarouselItems = contentGridCarouselItems });
            }

            var categories = await this.catalogService.GetCatalogCategoriesAsync(componentModel.Language, Foundation.GenericRepository.Definitions.Constants.DefaultPageIndex, Foundation.GenericRepository.Definitions.Constants.DefaultItemsPerPage);

            foreach (var category in categories.OrEmptyIfNull().Where(x => x.Level == CategoriesConstants.FirstLevel))
            {
                var contentGridCarouselItems = new List<ContentGridCarouselItemViewModel>();

                foreach (var subCategory in categories.Where(x => x.Level == CategoriesConstants.SecondLevel && x.ParentId == category.Id))
                {
                    var carouselItem = new ContentGridCarouselItemViewModel
                    { 
                        Id = subCategory.Id,
                        Title = subCategory.Name,
                        ImageAlt = subCategory.Name,
                        Url = this.linkGenerator.GetPathByAction("Index", "Category", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, subCategory.Id })
                    };

                    if (subCategory.ThumbnailMediaId.HasValue)
                    {
                        carouselItem.ImageUrl = this.mediaService.GetFileUrl(this.options.Value.MediaUrl, subCategory.ThumbnailMediaId.Value, true);
                    }

                    contentGridCarouselItems.Add(carouselItem);
                }

                items.Add(new ContentGridItemViewModel { Id = category.Id, Title = category.Name, CarouselItems = contentGridCarouselItems });
            }

            var viewModel = new HomePageContentGridViewModel
            {
                Items = items
            };

            return viewModel;
        }
    }
}
