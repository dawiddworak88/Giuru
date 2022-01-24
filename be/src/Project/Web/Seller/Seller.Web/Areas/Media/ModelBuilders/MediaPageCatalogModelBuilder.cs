using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.Extensions.Services.MediaServices;
using Foundation.GenericRepository.Definitions;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Seller.Web.Areas.Media.DomainModels;
using Seller.Web.Areas.Media.Repositories.Media;
using Seller.Web.Areas.Products.Repositories;
using Seller.Web.Shared.Catalogs.ModelBuilders;
using Seller.Web.Shared.Configurations;
using Seller.Web.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Media.ModelBuilders
{
    public class MediaPageCatalogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<MediaItem>>
    {
        private readonly ICatalogModelBuilder catalogModelBuilder;
        private readonly IStringLocalizer globalLocalizer;
        private readonly IStringLocalizer mediaLocalizer;
        private readonly LinkGenerator linkGenerator;
        private readonly IMediaRepository mediaRepository;
        private readonly IOptions<AppSettings> options;
        private readonly IMediaHelperService mediaService;

        public MediaPageCatalogModelBuilder(
            ICatalogModelBuilder catalogModelBuilder,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<MediaResources> mediaLocalizer,
            IMediaRepository mediaRepository,
            IOptions<AppSettings> options,
            IMediaHelperService mediaService,
            LinkGenerator linkGenerator)
        {
            this.catalogModelBuilder = catalogModelBuilder;
            this.globalLocalizer = globalLocalizer;
            this.mediaLocalizer = mediaLocalizer;
            this.linkGenerator = linkGenerator;
            this.mediaRepository = mediaRepository;
            this.mediaService = mediaService;
            this.options = options;
        }

        public async Task<CatalogViewModel<MediaItem>> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = this.catalogModelBuilder.BuildModel<CatalogViewModel<MediaItem>, MediaItem>();

            viewModel.Title = this.mediaLocalizer.GetString("Media");
            viewModel.NewText = this.mediaLocalizer.GetString("NewText");
            viewModel.NewUrl = this.linkGenerator.GetPathByAction("Edit", "Warehouse", new { Area = "Inventory", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.EditUrl = this.linkGenerator.GetPathByAction("Edit", "Warehouse", new { Area = "Inventory", culture = CultureInfo.CurrentUICulture.Name });

            viewModel.DeleteApiUrl = this.linkGenerator.GetPathByAction("Delete", "WarehousesApi", new { Area = "Inventory", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.SearchApiUrl = this.linkGenerator.GetPathByAction("Get", "MediaApi", new { Area = "Media", culture = CultureInfo.CurrentUICulture.Name });

            viewModel.OrderBy = $"{nameof(MediaItem.CreatedDate)} desc";

            viewModel.Table = new CatalogTableViewModel
            {
                Labels = new string[]
                {
                    this.globalLocalizer.GetString("Name"),
                    this.globalLocalizer.GetString("LastModifiedDate"),
                    this.globalLocalizer.GetString("CreatedDate")
                },
                Actions = new List<CatalogActionViewModel>
                {
                    new CatalogActionViewModel
                    {
                        IsEdit = false
                    },
                    new CatalogActionViewModel
                    {
                        IsDelete = true
                    }
                },
                Properties = new List<CatalogPropertyViewModel>
                {
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(MediaItem.FileName).ToCamelCase(),
                        IsPicture = true
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(MediaItem.LastModifiedDate).ToCamelCase(),
                        IsDateTime = true
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(MediaItem.CreatedDate).ToCamelCase(),
                        IsDateTime = true
                    }
                }
            };

            viewModel.PagedItems = await this.mediaRepository.GetMediaItemsAsync(componentModel.Token, componentModel.Language, null, Constants.DefaultPageIndex, Constants.DefaultItemsPerPage, $"{nameof(MediaItem.CreatedDate)} desc");

            return viewModel;
        }
    }
}