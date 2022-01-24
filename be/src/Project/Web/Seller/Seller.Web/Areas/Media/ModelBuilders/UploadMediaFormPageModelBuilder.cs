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
                Title = this.mediaResources.GetString("UploadMedia"),
                SaveMediaUrl = this.linkGenerator.GetPathByAction("Post", "FilesApi", new { Area = "Media", culture = CultureInfo.CurrentUICulture.Name }),
                DropFilesLabel = "asdasd",
                DropOrSelectImagesLabel = "Drop or select",
                MediaItemsLabel = "Media items lable",
                SaveMediaText = "AAA",
                GeneralErrorMessage = this.globalLocalizer.GetString("GeneralErrorMessage"),
                DeleteLabel = this.globalLocalizer.GetString("Delete")
            };

            return viewModel;
        }
    }
}
