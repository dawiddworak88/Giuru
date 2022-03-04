using Buyer.Web.Areas.News.ViewModel;
using Buyer.Web.Shared.Configurations;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.News.ModelBuilders
{
    public class NewsCatalogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, NewsCatalogViewModel>
    {
        private readonly IOptions<AppSettings> options;
        private readonly LinkGenerator linkGenerator;

        public NewsCatalogModelBuilder(
            IOptions<AppSettings> options,
            LinkGenerator linkGenerator)
        {
            this.options = options;
            this.linkGenerator = linkGenerator;
        }

        public async Task<NewsCatalogViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new NewsCatalogViewModel
            {
                NewsApiUrl = this.linkGenerator.GetPathByAction("Get", "NewsApi", new { Area = "News", culture = CultureInfo.CurrentUICulture.Name }),
            };

            return viewModel;
        }
    }
}
