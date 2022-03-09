using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.News.ApiRequestModels;
using Seller.Web.Areas.News.DomainModels;
using Seller.Web.Areas.News.Repositories.News;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.News.ApiControllers
{
    [Area("News")]
    public class NewsApiController : BaseApiController
    {
        private readonly IStringLocalizer newsLocalizer;
        private readonly INewsRepository newsRepository;

        public NewsApiController(
            IStringLocalizer<NewsResources> newsLocalizer,
            INewsRepository newsRepository)
        {
            this.newsLocalizer = newsLocalizer;
            this.newsRepository = newsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int pageIndex, int itemsPerPage)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var categories = await this.newsRepository.GetNewsItemsAsync(
                token, language, searchTerm, pageIndex, itemsPerPage, $"{nameof(NewsItem.CreatedDate)} desc");

            return this.StatusCode((int)HttpStatusCode.OK, categories);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NewsRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var news = await this.newsRepository.SaveAsync(
                token, language, model.Id, model.ThumbnailImageId, model.CategoryId, model.PreviewImageId, model.Title, 
                model.Description, model.Content, model.IsPublished, model.Files?.Select(x => x.Id)); 

            return this.StatusCode((int)HttpStatusCode.OK, new { news, Message = this.newsLocalizer.GetString("NewsSavedSuccessfully").Value });

        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await this.newsRepository.DeleteAsync(token, language, id);

            return this.StatusCode((int)HttpStatusCode.OK, new { Message = this.newsLocalizer.GetString("NewsDeletedSuccessfully").Value });
        }
    }
}
