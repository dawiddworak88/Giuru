using Buyer.Web.Areas.Home.DomainModels;
using Buyer.Web.Areas.Home.ViewModel;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Repositories.News;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Paginations;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Home.ModelBuilders
{
    public class HomePageNewsModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, NewsViewModel>
    {
        private readonly IOptions<AppSettings> options;
        private readonly INewsRepository newsRepository;

        public HomePageNewsModelBuilder(
            IOptions<AppSettings> options,
            INewsRepository newsRepository)
        {
            this.options = options;
            this.newsRepository = newsRepository;
        }

        public async Task<NewsViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new NewsViewModel
            {
                
            };

            var news = await this.newsRepository.GetNewsItemsAsync(componentModel.Token, componentModel.Language, 1, 6, "B2B", $"{nameof(NewsItem.CreatedDate)} desc");
            if (news is not null)
            {
                var pagedResults = new PagedResults<IEnumerable<NewsItemViewModel>>(news.Total, news.PageSize)
                {
                    Data = news.Data.Select(x => new NewsItemViewModel
                    {
                        Title = x.Title,

                    })
                };

                viewModel.NewsItems = pagedResults;
            }
            

            return viewModel;
        }
    }
}
