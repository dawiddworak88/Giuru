using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.ListItems.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.News.Repositories.Categories;
using Seller.Web.Areas.News.Repositories.News;
using Seller.Web.Areas.News.ViewModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Web.Areas.News.ModelBuilders
{
    public class NewsItemFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, NewsItemFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<NewsResources> newsLocalizer;
        private readonly LinkGenerator linkGenerator;
        private readonly ICategoriesRepository categoriesRepository;
        private readonly INewsRepository newsRepository;

        public NewsItemFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<NewsResources> newsLocalizer,
            ICategoriesRepository categoriesRepository,
            INewsRepository newsRepository,
            LinkGenerator linkGenerator)
        {
            this.linkGenerator = linkGenerator;
            this.globalLocalizer = globalLocalizer;
            this.newsLocalizer = newsLocalizer;
            this.categoriesRepository = categoriesRepository;
            this.newsRepository = newsRepository;
        }

        public async Task<NewsItemFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new NewsItemFormViewModel
            {
                Title = this.newsLocalizer.GetString("NewsItem"),
                SaveText = this.globalLocalizer.GetString("SaveText"),
                SaveUrl = this.linkGenerator.GetPathByAction("Post", "NewsApi", new { Area = "News", culture = CultureInfo.CurrentUICulture.Name }),
                SaveMediaUrl = this.linkGenerator.GetPathByAction("Post", "FilesApi", new { Area = "Media", culture = CultureInfo.CurrentUICulture.Name }),
                DeleteLabel = this.globalLocalizer.GetString("Delete"),
                GeneralErrorMessage = this.globalLocalizer.GetString("AnErrorOccurred"),
                DropOrSelectImagesLabel = this.globalLocalizer.GetString("DropOrSelectFile"),
                DropFilesLabel = this.globalLocalizer.GetString("DropOrSelectFile"),
                NewsUrl = this.linkGenerator.GetPathByAction("Index", "News", new { Area = "News", culture = CultureInfo.CurrentUICulture.Name }),
                NavigateToNewsLabel = this.newsLocalizer.GetString("NavigateToNewsLabel"),
                HeroImageLabel = this.newsLocalizer.GetString("HeroImageLabel"),
                TitleLabel = this.newsLocalizer.GetString("TitleLabel"),
                DescriptionLabel = this.newsLocalizer.GetString("DescriptionLabel"),
                IsNewLabel = this.newsLocalizer.GetString("IsNewLabel"),
                IsPublishedLabel = this.newsLocalizer.GetString("IsPublishedLabel"),
                ImagesLabel = this.newsLocalizer.GetString("ImagesLabel"),
                FilesLabel = this.newsLocalizer.GetString("FilesLabel"),
                CategoryLabel = this.newsLocalizer.GetString("Category"),
                SelectCategoryLabel = this.newsLocalizer.GetString("SelectCategoryLabel"),
                ThumbImageLabel = this.newsLocalizer.GetString("ThumbImageLabel"),
                TitleRequiredErrorMessage = this.newsLocalizer.GetString("TitleRequiredErrorMessage"),
                CategoryRequiredErrorMessage = this.newsLocalizer.GetString("CategoryRequiredErrorMessage"),
                DescriptionRequiredErrorMessage = this.newsLocalizer.GetString("DescriptionRequiredErrorMessage")
            };

            var categories = await this.categoriesRepository.GetAllCategoriesAsync(componentModel.Token, componentModel.Language);
            if (categories is not null)
            {
                viewModel.Categories = categories.Select(x => new ListItemViewModel { Id = x.Id, Name = x.Name });
            }

            if (componentModel.Id.HasValue)
            {
                var existingNews = await this.newsRepository.GetAsync(componentModel.Token, componentModel.Language, componentModel.Id);
                if (existingNews is not null)
                {
                    viewModel.Id = componentModel.Id;
                    viewModel.ThumbImageId = existingNews.ThumbImageId;
                    viewModel.HeroImageId = existingNews.HeroImageId;
                    viewModel.CategoryId = existingNews.CategoryId;
                    viewModel.NewsTitle = existingNews.Title;
                    viewModel.Content = existingNews.Content;
                    viewModel.Description = existingNews.Description;
                    viewModel.IsPublished = existingNews.IsPublished;
                    viewModel.Files = existingNews.Files;
                }
            }

            return viewModel;
        }
    }
}
