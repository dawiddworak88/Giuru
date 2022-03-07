using Buyer.Web.Areas.News.Repositories;
using Buyer.Web.Areas.News.ViewModel;
using Buyer.Web.Shared.ModelBuilders.Breadcrumbs;
using Buyer.Web.Shared.ViewModels.Breadcrumbs;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using System.Globalization;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.News.ModelBuilders
{
    public class NewsItemBreadcrumbsModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, NewsItemBreadcrumbsViewModel>
    {
        private readonly LinkGenerator linkGenerator;
        private readonly INewsRepository newsRepository;
        private readonly IBreadcrumbsModelBuilder<ComponentModelBase, NewsItemBreadcrumbsViewModel> breadcrumbsModelBuilder;

        public NewsItemBreadcrumbsModelBuilder(
            LinkGenerator linkGenerator,
            INewsRepository newsRepository,
            IBreadcrumbsModelBuilder<ComponentModelBase, NewsItemBreadcrumbsViewModel> breadcrumbsModelBuilder)
        {
            this.linkGenerator = linkGenerator;
            this.breadcrumbsModelBuilder = breadcrumbsModelBuilder;
            this.newsRepository = newsRepository;
        }

        public async Task<NewsItemBreadcrumbsViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = this.breadcrumbsModelBuilder.BuildModel(componentModel);

            viewModel.Items.Add(new BreadcrumbViewModel
            {
                Name = "News",
                Url = this.linkGenerator.GetPathByAction("Index", "News", new { Area = "News", culture = CultureInfo.CurrentUICulture.Name }),
                IsActive = false,
            });

            var newsItem = await this.newsRepository.GetNewsItemAsync(componentModel.Token, componentModel.Language, componentModel.Id);
            if (newsItem is not null)
            {
                var newsBreadcrumb = new BreadcrumbViewModel
                {
                    Name = newsItem.Title,
                    Url = this.linkGenerator.GetPathByAction("Details", "NewsItem", new { Area = "News", culture = CultureInfo.CurrentUICulture.Name, Id = newsItem.Id }),
                    IsActive = true
                };

                viewModel.Items.Add(newsBreadcrumb);
            }

            return viewModel;
        }
    }
}
