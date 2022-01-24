using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Media.ViewModel;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Media.ModelBuilders
{
    public class UploadMediaFormPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, UploadMediaFormViewModel>
    {
        private readonly IStringLocalizer globalLocalizer;
        private readonly IStringLocalizer mediaResources;
        private readonly LinkGenerator linkGenerator;

        public UploadMediaFormPageModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<MediaResources> mediaResources,
            LinkGenerator linkGenerator)
        {
            this.globalLocalizer = globalLocalizer;
            this.mediaResources = mediaResources;
            this.linkGenerator = linkGenerator;
        }

        public async Task<UploadMediaFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new UploadMediaFormViewModel
            {
                Title = this.mediaResources.GetString("Media"),
                SaveMediaUrl = this.linkGenerator.GetPathByAction("Post", "FilesApi", new { Area = "Media", culture = CultureInfo.CurrentUICulture.Name }),
                DropFilesLabel = this.globalLocalizer.GetString("DropOrSelectFile"),
                DropOrSelectImagesLabel = this.globalLocalizer.GetString("DropOrSelectFile"),
                MediaItemsLabel = this.mediaResources.GetString("MediaItemsLabel"),
                BackToMediaText = this.mediaResources.GetString("BackToMediaText"),
                MediaUrl = this.linkGenerator.GetPathByAction("Index", "Media", new { Area = "Media", culture = CultureInfo.CurrentUICulture.Name }),
                GeneralErrorMessage = this.globalLocalizer.GetString("AnErrorOccurred"),
                DeleteLabel = this.globalLocalizer.GetString("Delete")
            };

            return viewModel;
        }
    }
}
