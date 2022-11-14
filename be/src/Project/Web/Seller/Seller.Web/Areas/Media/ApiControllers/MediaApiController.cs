using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Media.ApiRequestModels;
using Seller.Web.Areas.Media.DomainModels;
using Seller.Web.Areas.Media.Repositories.Media;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Media.ApiControllers
{
    [Area("Media")]
    public class MediaApiController : BaseApiController
    {
        private readonly IMediaRepository mediaRepository;
        private readonly IStringLocalizer<MediaResources> mediaLocalizer;

        public MediaApiController(
            IMediaRepository mediaRepository,
            IStringLocalizer<MediaResources> mediaLocalizer)
        {
            this.mediaRepository = mediaRepository;
            this.mediaLocalizer = mediaLocalizer;
        }

        [HttpPost]
        public async Task<IActionResult> SaveGroups([FromBody] MediaItemGroupsRequestModel request)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            if (request.Files.OrEmptyIfNull().Any())
            {
                foreach (var fileId in request.Files.Select(x => x.Id))
                {
                    await this.mediaRepository.SaveMediaItemGroupsAsync(token, language, fileId, request.ClientGroupIds);
                }

                return this.StatusCode((int)HttpStatusCode.OK, new { Message = this.mediaLocalizer.GetString("SuccessfullySavedMedia").Value });
            }

            return this.StatusCode((int)HttpStatusCode.BadRequest);
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
                model.Id, model.Name, model.Description, model.MetaData, model.ClientGroupIds, token, language);

            return this.StatusCode((int)HttpStatusCode.OK, new { Message = this.mediaLocalizer.GetString("MediaUpdatedSuccessfully").Value });
        }
    }
}
