using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Media.ApiRequestModels;
using Seller.Web.Areas.Media.DomainModels;
using Seller.Web.Areas.Media.Repositories.Media;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Media.ApiControllers
{
    [Area("Media")]
    public class MediaApiController : BaseApiController
    {
        private readonly IMediasRepository mediaRepository;
        private readonly IStringLocalizer<MediaResources> mediaLocalizer;

        public MediaApiController(
            IMediasRepository mediaRepository,
            IStringLocalizer<MediaResources> mediaLocalizer)
        {
            this.mediaRepository = mediaRepository;
            this.mediaLocalizer = mediaLocalizer;
        }


        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int pageIndex, int itemsPerPage)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var inventories = await this.mediaRepository.GetMediaItemsAsync(
                token, language, searchTerm, pageIndex, itemsPerPage, $"{nameof(MediaItem.CreatedDate)} DESC");

            return this.StatusCode((int)HttpStatusCode.OK, inventories);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateVersion([FromBody] UpdateMediaItemVersionRequestModel model )
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await this.mediaRepository.UpdateMediaItemVersionAsync(
                model.Id, model.Name, model.Description, model.MetaData, token, language);

            return this.StatusCode((int)HttpStatusCode.OK, new { Message = this.mediaLocalizer.GetString("MediaUpdatedSuccessfully").Value });
        }
    }
}
