using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Definitions;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Media.DomainModels;
using Seller.Web.Areas.Media.Repositories.Media;
using Seller.Web.Shared.Catalogs.ModelBuilders;
using Seller.Web.Shared.ViewModels;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Media.ModelBuilders
{
    public class MediaItemsPageCatalogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<MediaItem>>
    {
        private readonly ICatalogModelBuilder catalogModelBuilder;
        private readonly IStringLocalizer globalLocalizer;
        private readonly IStringLocalizer mediaLocalizer;
        private readonly LinkGenerator linkGenerator;
        private readonly IMediaRepository mediaRepository;

        public MediaItemsPageCatalogModelBuilder(
            ICatalogModelBuilder catalogModelBuilder,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<MediaResources> mediaLocalizer,
            IMediaRepository mediaRepository,
            LinkGenerator linkGenerator)
        {
            this.catalogModelBuilder = catalogModelBuilder;
            this.globalLocalizer = globalLocalizer;
            this.mediaLocalizer = mediaLocalizer;
            this.linkGenerator = linkGenerator;
            this.mediaRepository = mediaRepository;
        }

        public async Task<CatalogViewModel<MediaItem>> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = this.catalogModelBuilder.BuildModel<CatalogViewModel<MediaItem>, MediaItem>();

            viewModel.Title = this.mediaLocalizer.GetString("Media");
            viewModel.NewText = this.mediaLocalizer.GetString("NewText");
            viewModel.IsAttachmentLabel = this.mediaLocalizer.GetString("IsAttachmentLabel");

            viewModel.NewUrl = this.linkGenerator.GetPathByAction("Index", "Media", new { Area = "Media", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.EditUrl = this.linkGenerator.GetPathByAction("Item", "MediaItems", new { Area = "Media", culture = CultureInfo.CurrentUICulture.Name });

            viewModel.DeleteApiUrl = this.linkGenerator.GetPathByAction("Delete", "FilesApi", new { Area = "Media", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.SearchApiUrl = this.linkGenerator.GetPathByAction("Get", "MediaApi", new { Area = "Media", culture = CultureInfo.CurrentUICulture.Name });

            viewModel.OrderBy = $"{nameof(MediaItem.CreatedDate)} desc";

            viewModel.Table = new CatalogTableViewModel
            {
                Labels = new string[]
                {
                    this.globalLocalizer.GetString("Miniature"),
                    this.globalLocalizer.GetString("Filename"),
                    this.globalLocalizer.GetString("LastModifiedDate"),
                    this.globalLocalizer.GetString("CreatedDate")
                },
                Actions = new List<CatalogActionViewModel>
                {
                    new CatalogActionViewModel
                    {
                        IsEdit = true
                    },
                    new CatalogActionViewModel
                    {
                        IsDelete = true
                    },
                    new CatalogActionViewModel
                    {
                        isPicture = true
                    }
                },
                Properties = new List<CatalogPropertyViewModel>
                {
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(MediaItem.Url).ToCamelCase(),
                        IsPicture = true
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(MediaItem.Name).ToCamelCase(),
                        IsDateTime = false,
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