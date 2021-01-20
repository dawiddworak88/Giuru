using Foundation.ApiExtensions.Controllers;
using Foundation.Extensions.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Media.Api.v1.Areas.Media.Models;
using Media.Api.v1.Areas.Media.Services;
using Media.Api.v1.Areas.Media.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Media.Api.v1.Areas.Media.Controllers
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
        /// <param name="pageIndex">The page index.</param>
        /// <param name="itemsPerPage">The number of items per page.</param>
        /// <returns>Media items.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType(400)]
        [AllowAnonymous]
        public async Task<IActionResult> Get(string ids, int pageIndex, int itemsPerPage)
        {
            var mediaItemsIds = ids.ToEnumerableGuidIds();

            if (mediaItemsIds != null)
            {
                var serviceModel = new GetMediaItemsByIdsModel
                {
                    Language = CultureInfo.CurrentCulture.Name,
                    Ids = mediaItemsIds,
                    PageIndex = pageIndex,
                    ItemsPerPage = itemsPerPage
                };

                var validator = new GetMediaItemsByIdsModelValidator();

                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var mediaItems = this.mediaService.GetMediaItemsByIds(serviceModel);

                    return this.StatusCode((int)HttpStatusCode.OK, mediaItems);
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }

            return this.BadRequest();
        }

        /// <summary>
        /// Gets media item by id.
        /// </summary>
        /// <param name="id">The media item id.</param>
        /// <returns>The media item.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [AllowAnonymous]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Get(Guid? id)
        {
            var serviceModel = new GetMediaItemsByIdModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name
            };

            var validator = new GetMediaItemsByIdModelValidator();

            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var mediaItems = this.mediaService.GetMediaItemById(serviceModel);

                return this.StatusCode((int)HttpStatusCode.OK, mediaItems);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }
    }
}
