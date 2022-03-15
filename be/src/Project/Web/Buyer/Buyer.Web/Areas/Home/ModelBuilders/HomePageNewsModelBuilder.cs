using Buyer.Web.Areas.Home.Definitions;
using Buyer.Web.Areas.Home.DomainModels;
using Buyer.Web.Areas.Home.ViewModel;
using Buyer.Web.Shared.Repositories.News;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Home.ModelBuilders
{
    public class HomePageNewsModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, NewsViewModel>
    {
        private readonly INewsRepository newsRepository;
        private readonly IStringLocalizer<NewsResources> newsLocalizer;

        public HomePageNewsModelBuilder(
            INewsRepository newsRepository,
            IStringLocalizer<NewsResources> newsLocalizer)
        {
            this.newsRepository = newsRepository;
            this.newsLocalizer = newsLocalizer;
        }

        public async Task<NewsViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new NewsViewModel
            {
                Title = this.newsLocalizer.GetString("News")
            };

            var news = await this.newsRepository.GetNewsItemsAsync(
                componentModel.Token, componentModel.Language, HomeConstants.News.DefaultPageIndex, HomeConstants.News.DefaultPageSize, HomeConstants.News.DefaultSearchTerm, $"{nameof(NewsItem.CreatedDate)} desc");

            if (news is not null)
            {
                var pagedResults = new PagedResults<IEnumerable<NewsItemViewModel>>(news.Total, news.PageSize)
                {
                    Data = news.Data.Select(x => new NewsItemViewModel
                    {
                        Title = x.Title,
                        CategoryName = x.CategoryName,
                        Description = x.Description,
                        Content = x.Content,
                        Url = x.Url,
                        ThumbImageUrl = x.ThumbImageUrl,
                        ThumbImages = x.ThumbImages,
                        LastModifiedDate = x.LastModifiedDate,
                        CreatedDate = x.CreatedDate
                    })
                };

                viewModel.PagedResults = pagedResults;
            }

            return viewModel;
        }
    }
}
