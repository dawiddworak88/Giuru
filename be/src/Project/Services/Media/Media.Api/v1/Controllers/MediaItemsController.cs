using Foundation.ApiExtensions.Controllers;
using Foundation.Extensions.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Paginations;
using Media.Api.Services.Media;
using Media.Api.ServicesModels;
using Media.Api.v1.ResponseModels;
using Media.Api.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Media.Api.v1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PagedResults<IEnumerable<MediaItemResponseModel>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> Get(string ids, int pageIndex, int itemsPerPage)
        {
            var mediaItemsIds = ids.ToEnumerableGuidIds();

            if (mediaItemsIds != null)
            {
                var serviceModel = new GetMediaItemsByIdsServiceModel
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

                    if (mediaItems != null)
                    {
                        var response = new PagedResults<IEnumerable<MediaItemResponseModel>>(mediaItems.Total, mediaItems.PageSize)
                        { 
                            Data = mediaItems.Data.OrEmptyIfNull().Select(x => new MediaItemResponseModel
                            {
                                Id = x.Id,
                                Description = x.Description,
                                Extension = x.Extension,
                                Filename = x.Filename,
                                IsProtected = x.IsProtected,
                                MimeType = x.MimeType,
                                Name = x.Name,
                                Size = x.Size,
                                LastModifiedDate = x.LastModifiedDate,
                                CreatedDate = x.CreatedDate
                            })
                        };

                        return this.StatusCode((int)HttpStatusCode.OK, response);
                    }
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(MediaItemResponseModel))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Get(Guid? id)
        {
            var serviceModel = new GetMediaItemsByIdServiceModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name
            };

            var validator = new GetMediaItemsByIdModelValidator();

            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var mediaItem = this.mediaService.GetMediaItemById(serviceModel);

                if (mediaItem != null)
                {
                    var response = new MediaItemResponseModel
                    {
                        Id = mediaItem.Id,
                        Description = mediaItem.Description,
                        Extension = mediaItem.Extension,
                        Filename = mediaItem.Filename,
                        IsProtected = mediaItem.IsProtected,
                        MimeType = mediaItem.MimeType,
                        Name = mediaItem.Name,
                        Size = mediaItem.Size,
                        LastModifiedDate = mediaItem.LastModifiedDate,
                        CreatedDate = mediaItem.CreatedDate
                    };

                    return this.StatusCode((int)HttpStatusCode.OK, response);
                }
                else
                {
                    return this.StatusCode((int)HttpStatusCode.NotFound);
                }                
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }
    }
}
