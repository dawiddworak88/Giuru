using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Definitions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Media.ViewModel;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Media.ModelBuilders
{
    public class MediaFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, MediaFormViewModel>
    {
        private readonly IStringLocalizer globalLocalizer;
        private readonly IStringLocalizer mediaResources;
        private readonly LinkGenerator linkGenerator;

        public MediaFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<MediaResources> mediaResources,
            LinkGenerator linkGenerator)
        {
            this.globalLocalizer = globalLocalizer;
            this.mediaResources = mediaResources;
            this.linkGenerator = linkGenerator;
        }

        public async Task<MediaFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new MediaFormViewModel
            {
                Title = this.mediaResources.GetString("Media"),
                SaveMediaUrl = this.linkGenerator.GetPathByAction("Post", "FilesApi", new { Area = "Media", culture = CultureInfo.CurrentUICulture.Name }),
                DropFilesLabel = this.globalLocalizer.GetString("DropOrSelectFile"),
                DropOrSelectImagesLabel = this.globalLocalizer.GetString("DropOrSelectFile"),
                MediaItemsLabel = this.mediaResources.GetString("MediaItemsLabel"),
                BackToMediaText = this.mediaResources.GetString("BackToMediaText"),
                MediaUrl = this.linkGenerator.GetPathByAction("Index", "MediaItems", new { Area = "Media", culture = CultureInfo.CurrentUICulture.Name }),
                GeneralErrorMessage = this.globalLocalizer.GetString("AnErrorOccurred"),
                DeleteLabel = this.globalLocalizer.GetString("Delete"),
                SaveMediaChunkUrl = this.linkGenerator.GetPathByAction("PostChunk", "FilesApi", new { Area = "Media", culture = CultureInfo.CurrentUICulture.Name }),
                SaveMediaChunkCompleteUrl = this.linkGenerator.GetPathByAction("PostChunksComplete", "FilesApi", new { Area = "Media", culture = CultureInfo.CurrentUICulture.Name }),
                IsUploadInChunksEnabled = true,
                ChunkSize = MediaConstants.DefaultChunkSize,
            };

            return viewModel;
        }
    }
}
