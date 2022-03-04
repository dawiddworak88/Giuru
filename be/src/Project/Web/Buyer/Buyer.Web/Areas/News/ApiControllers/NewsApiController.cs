using Buyer.Web.Areas.News.DomainModels;
using Buyer.Web.Areas.News.Repositories;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.News.ApiControllers
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
        public async Task<IActionResult> Get(int pageIndex, int itemsPerPage)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var categories = await this.newsRepository.GetNewsItemsAsync(
                token, language, pageIndex, itemsPerPage, $"{nameof(NewsItem.CreatedDate)} desc");

            return this.StatusCode((int)HttpStatusCode.OK, categories);
        }
    }
}
