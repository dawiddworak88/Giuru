using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.News.ViewModel;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.News.ModelBuilders
{
    public class NewsItemFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, NewsItemFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<NewsResources> newsLocalizer;
        private readonly LinkGenerator linkGenerator;

        public NewsItemFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<NewsResources> newsLocalizer,
            LinkGenerator linkGenerator)
        {
            this.linkGenerator = linkGenerator;
            this.globalLocalizer = globalLocalizer;
            this.newsLocalizer = newsLocalizer;
        }

        public async Task<NewsItemFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new NewsItemFormViewModel
            {
                Title = this.newsLocalizer.GetString("NewsItem"),
                SaveText = this.globalLocalizer.GetString("SaveText"),
                SaveUrl = this.linkGenerator.GetPathByAction("Post", "NewsItemApi", new { Area = "News", culture = CultureInfo.CurrentUICulture.Name }),
                DeleteLabel = this.globalLocalizer.GetString("Delete"),
                GeneralErrorMessage = this.globalLocalizer.GetString("AnErrorOccurred"),
                DropOrSelectImagesLabel = this.globalLocalizer.GetString("DropOrSelectFile"),
                DropFilesLabel = this.globalLocalizer.GetString("DropOrSelectFile")
            };

            return viewModel;
        }
    }
}
