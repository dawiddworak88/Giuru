using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Controllers;
using Foundation.Extensions.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.Helpers;
using Foundation.GenericRepository.Paginations;
using Media.Api.Services.Media;
using Media.Api.ServicesModels;
using Media.Api.v1.Areas.Media.ResultModels;
using Media.Api.v1.RequestModels;
using Media.Api.v1.ResponseModels;
using Media.Api.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
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
        /// <param name="searchTerm">The search term.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="itemsPerPage">The number of items per page.</param>
        /// <param name="orderBy">The optional order by.</param>
        /// <returns>Media items.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PagedResults<IEnumerable<MediaItemResponseModel>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> Get(string ids, string searchTerm, int pageIndex, int itemsPerPage, string orderBy )
        {
            var sellerClaims = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);
            var mediaItemsIds = ids.ToEnumerableGuidIds();
            if (mediaItemsIds != null)
            {
                var serviceModel = new GetMediaItemsByIdsServiceModel
                {
                    Language = CultureInfo.CurrentCulture.Name,
                    Ids = mediaItemsIds,
                    PageIndex = pageIndex,
                    ItemsPerPage = itemsPerPage,
                    Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaims?.Value)
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
                                Name = x.Name,
                                Extension = x.Extension,
                                Filename = x.Filename,
                                MimeType = x.MimeType,
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
            else
            {
                var serviceModel = new GetMediaItemsServiceModel
                {
                    Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaims?.Value),
                    Language = CultureInfo.CurrentCulture.Name,
                    SearchTerm = searchTerm,
                    PageIndex = pageIndex,
                    ItemsPerPage = itemsPerPage,
                    OrderBy = orderBy,
                };

                var mediaItems = this.mediaService.GetAsync(serviceModel);
                if (mediaItems != null)
                {
                    var response = new PagedResults<IEnumerable<MediaItemResponseModel>>(mediaItems.Result.Total, mediaItems.Result.PageSize)
                    {
                        Data = mediaItems.Result.Data.OrEmptyIfNull().Select(x => new MediaItemResponseModel
                        {
                            Id = x.Id,
                            Description = x.Description,
                            Extension = x.Extension,
                            Filename = x.Filename,
                            MimeType = x.MimeType,
                            Name = x.Name,
                            Size = x.Size,
                            LastModifiedDate = x.LastModifiedDate,
                            CreatedDate = x.CreatedDate
                        })
                    };

                    return this.StatusCode((int)HttpStatusCode.OK, response);
                }
                throw new CustomException("", (int)HttpStatusCode.UnprocessableEntity);
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
                        MetaData = mediaItem.MetaData,
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

        /// <summary>
        /// Updates media item version data
        /// </summary>
        /// <param name="request">The model.</param>
        /// <returns>Created if the file has been uploaded successfully.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [Route("versions")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Guid))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> UpdateVersion(UpdateMediaItemVersionRequestModel request)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);
            var serviceModel = new UpdateMediaItemVersionServiceModel
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
                Language = CultureInfo.CurrentCulture.Name,
                MetaData = request.MetaData,
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new UpdateMediaItemVersionModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);
            if (validationResult.IsValid)
            {
                await this.mediaService.UpdateMediaItemVersionAsync(serviceModel);
                
                return this.StatusCode((int)HttpStatusCode.OK);
            }
            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Gets media item versions by media item id.
        /// </summary>
        /// <param name="id">The media item id.</param>
        /// <returns>The media item versions.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [AllowAnonymous]
        [Route("versions/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(MediaItemResponseModel))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetVersions(Guid? id)
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
                var mediaItemVersions = await this.mediaService.GetMediaItemVerionsByIdAsync(serviceModel);
                if (mediaItemVersions is not null)
                {
                    var response = new MediaItemVerionsByIdServiceModel
                    {
                        Id = mediaItemVersions.Id,
                        Name = mediaItemVersions.Name,
                        Description = mediaItemVersions.Description,
                        MetaData = mediaItemVersions.MetaData,
                        Versions = mediaItemVersions.Versions,
                    };
                    return this.StatusCode((int)HttpStatusCode.OK, mediaItemVersions);
                }
                return this.StatusCode((int)HttpStatusCode.NotFound);
            }
            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }
    }
}
