using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IMediaRepository mediaRepository;

        public MediaApiController(
            IMediaRepository mediaRepository)
        {
            this.mediaRepository = mediaRepository;
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
    }
}
