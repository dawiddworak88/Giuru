using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Definitions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Media.Repositories.Media;
using Seller.Web.Areas.Media.ViewModel;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Media.ModelBuilders
{
    public class MediaItemFormPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, MediaItemFormViewModel>
    {
        private readonly IStringLocalizer globalLocalizer;
        private readonly IStringLocalizer mediaResources;
        private readonly IMediaRepository mediaRepository;
        private readonly LinkGenerator linkGenerator;

        public MediaItemFormPageModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<MediaResources> mediaResources,
            IMediaRepository mediaRepository,
            LinkGenerator linkGenerator)
        {
            this.globalLocalizer = globalLocalizer;
            this.mediaResources = mediaResources;
            this.linkGenerator = linkGenerator;
            this.mediaRepository = mediaRepository;
        }

        public async Task<MediaItemFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new MediaItemFormViewModel
            {
                Title = this.mediaResources.GetString("Media"),
                SaveMediaUrl = this.linkGenerator.GetPathByAction("Post", "FilesApi", new { Area = "Media", culture = CultureInfo.CurrentUICulture.Name }),
                DropFilesLabel = this.globalLocalizer.GetString("DropOrSelectFile"),
                DropOrSelectImagesLabel = this.globalLocalizer.GetString("DropOrSelectFile"),
                MediaItemsLabel = this.mediaResources.GetString("MediaItemsLabel"),
                BackToMediaText = this.mediaResources.GetString("BackToMediaText"),
                GeneralErrorMessage = this.globalLocalizer.GetString("AnErrorOccurred"),
                DeleteLabel = this.globalLocalizer.GetString("Delete"),
                NameLabel = this.globalLocalizer.GetString("Name"),
                DescriptionLabel = this.globalLocalizer.GetString("Description"),
                SaveMediaText = this.globalLocalizer.GetString("SaveText"),
                UpdateMediaVersionUrl = this.linkGenerator.GetPathByAction("UpdateVersion", "MediaApi", new { Area = "Media", culture = CultureInfo.CurrentUICulture.Name }),
                LatestVersionsLabel = this.mediaResources.GetString("LatestVersionsLabel"),
                MetaDataLabel = this.mediaResources.GetString("MetaDataLabel"),
                IdLabel = this.globalLocalizer.GetString("Id"),
                MediaUrl = this.linkGenerator.GetPathByAction("Index", "MediaItems", new { Area = "Media", culture = CultureInfo.CurrentUICulture.Name }),
                SaveMediaChunkUrl = this.linkGenerator.GetPathByAction("PostChunk", "FilesApi", new { Area = "Media", culture = CultureInfo.CurrentUICulture.Name }),
                SaveMediaChunkCompleteUrl = this.linkGenerator.GetPathByAction("PostChunksComplete", "FilesApi", new { Area = "Media", culture = CultureInfo.CurrentUICulture.Name }),
                IsUploadInChunksEnabled = true,
                ChunkSize = MediaConstants.DefaultChunkSize,
            };

            if (componentModel.Id.HasValue)
            {
                var itemVersions = await this.mediaRepository.GetMediaItemVersionsAsync(componentModel.Id.Value, componentModel.Token, componentModel.Language);

                if (itemVersions is not null)
                {
                    viewModel.Id = componentModel.Id.Value;
                    viewModel.Name = itemVersions.Name;
                    viewModel.Description = itemVersions.Description;
                    viewModel.Versions = itemVersions.Versions;
                    viewModel.MetaData = itemVersions.MetaData;
                }
            }

            return viewModel;
        }
    }
}
