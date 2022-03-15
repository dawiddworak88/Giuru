using Buyer.Web.Areas.News.Definitions;
using Buyer.Web.Areas.News.DomainModels;
using Buyer.Web.Areas.News.Repositories.Categories;
using Buyer.Web.Areas.News.ViewModel;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Repositories.News;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.ListItems.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.News.ModelBuilders
{
    public class NewsCatalogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, NewsCatalogViewModel>
    {
        private readonly IOptions<AppSettings> options;
        private readonly LinkGenerator linkGenerator;
        private readonly ICategoriesRepository categoriesRepository;
        private readonly INewsRepository newsRepository;
        private readonly IStringLocalizer<NewsResources> newsLocalizer;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;

        public NewsCatalogModelBuilder(
            IOptions<AppSettings> options,
            ICategoriesRepository categoriesRepository,
            IStringLocalizer<NewsResources> newsLocalizer,
            IStringLocalizer<GlobalResources> globalLocalizer,
            INewsRepository newsRepository,
            LinkGenerator linkGenerator)
        {
            this.options = options;
            this.linkGenerator = linkGenerator;
            this.categoriesRepository = categoriesRepository;
            this.newsLocalizer = newsLocalizer;
            this.globalLocalizer = globalLocalizer;
            this.newsRepository = newsRepository;
        }

        public async Task<NewsCatalogViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new NewsCatalogViewModel
            {
                NewsApiUrl = this.linkGenerator.GetPathByAction("Get", "NewsApi", new { Area = "News", culture = CultureInfo.CurrentUICulture.Name }),
                AllCategoryLabel = this.newsLocalizer.GetString("AllCategoryLabel"),
                NoResultsLabel = this.globalLocalizer.GetString("NoResultsLabel")
            };

            var news = await this.newsRepository.GetNewsItemsAsync(componentModel.Token, componentModel.Language, NewsConstants.DefaultPageIndex, NewsConstants.DefaultPageSize, null, $"{nameof(NewsItem.CreatedDate)} desc");
            if (news is not null)
            {
                var categories = new List<ListItemViewModel>();
                foreach(var newsItem in news.Data)
                {
                    var existingCategory = categories.FirstOrDefault(x => x.Name == newsItem.CategoryName);
                    if (existingCategory is null)
                    {
                        var category = new ListItemViewModel
                        {
                            Id = newsItem.CategoryId,
                            Name = newsItem.CategoryName
                        };

                        categories.Add(category);
                    }
                };

                viewModel.Categories = categories.OrderBy(x => x.Name);
                viewModel.PagedResults = news;
            }

            return viewModel;
        }
    }
}
