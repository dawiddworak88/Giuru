using Foundation.ApiExtensions.Controllers;
using Foundation.Extensions.ExtensionMethods;
using Media.Api.v1.Area.Media.Models;
using Media.Api.v1.Area.Media.Services;
using Media.Api.v1.Area.Media.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Media.Api.v1.Area.Media.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [AllowAnonymous]
    [ApiController]
    public class MediaItemsController : BaseApiController
    {
        private readonly IMediaService mediaService;

        public MediaItemsController(IMediaService mediaService)
        {
            this.mediaService = mediaService;
        }

        /// <summary>
        /// Gets media items by ids.
        /// </summary>
        /// <param name="ids">Ids of media items.</param>
        /// <param name="language">The language.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="itemsPerPage">The number of items per page.</param>
        /// <returns>Media items.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [AllowAnonymous]
        public async Task<IActionResult> Get(string ids, string language, int pageIndex, int itemsPerPage)
        {
            var mediaItemsIds = ids.ToEnumerableGuidIds();

            if (mediaItemsIds != null)
            {
                var serviceModel = new GetMediaItemsByIdsModel
                {
                    Ids = mediaItemsIds,
                    PageIndex = pageIndex,
                    ItemsPerPage = itemsPerPage,
                    Language = language
                };

                var validator = new GetMediaItemsByIdsModelValidator();

                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var mediaItems = this.mediaService.GetMediaItemsByIds(serviceModel);

                    return this.StatusCode((int)HttpStatusCode.OK, mediaItems);
                }
            }

            return this.BadRequest();
        }
    }
}
