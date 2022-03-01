using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.News.ApiRequestModels;
using Seller.Web.Areas.News.Repositories.News;
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

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NewsRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var category = await this.newsRepository.SaveAsync(
                token, language, model.Id, model.CategoryId, model.HeroImageId, model.Title, model.Description,
                model.Content, model.IsNew, model.IsPublished, model.Images?.Select(x => x.Id), model.Files?.Select(x => x.Id)); 

            return this.StatusCode((int)HttpStatusCode.OK, new { category, Message = this.newsLocalizer.GetString("NewsSavedSuccessfully").Value });

        }
    }
}
