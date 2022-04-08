using Buyer.Web.Areas.News.DomainModels;
using Buyer.Web.Shared.Repositories.News;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.News.ApiControllers
{
    [Area("News")]
    public class NewsApiController : BaseApiController
    {
        private readonly INewsRepository newsRepository;

        public NewsApiController(
            INewsRepository newsRepository)
        {
            this.newsRepository = newsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int pageIndex, int itemsPerPage)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var categories = await this.newsRepository.GetNewsItemsAsync(
                token, language, pageIndex, itemsPerPage, null, $"{nameof(NewsItem.CreatedDate)} desc");

            return this.StatusCode((int)HttpStatusCode.OK, categories);
        }
    }
}
