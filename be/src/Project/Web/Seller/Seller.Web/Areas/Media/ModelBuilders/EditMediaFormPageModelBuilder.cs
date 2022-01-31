using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Media.ViewModel;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Media.ModelBuilders
{
    public class EditMediaFormPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, EditMediaFormViewModel>
    {
        private readonly IStringLocalizer globalLocalizer;
        private readonly IStringLocalizer mediaResources;
        private readonly LinkGenerator linkGenerator;

        public EditMediaFormPageModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<MediaResources> mediaResources,
            LinkGenerator linkGenerator)
        {
            this.globalLocalizer = globalLocalizer;
            this.mediaResources = mediaResources;
            this.linkGenerator = linkGenerator;
        }

        public async Task<EditMediaFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new EditMediaFormViewModel
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
                Description = this.globalLocalizer.GetString("Description"),
                SaveMediaText = this.globalLocalizer.GetString("SaveText")
            };

            if (componentModel.Id.HasValue)
            {
                Console.WriteLine("HAS VALUE");
            }

            return viewModel;
        }
    }
}
