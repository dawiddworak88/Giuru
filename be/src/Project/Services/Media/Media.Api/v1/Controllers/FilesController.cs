using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Controllers;
using Foundation.Extensions.Definitions;
using Foundation.Extensions.Helpers;
using Media.Api.ServicesModels;
using Media.Api.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Media.Api.Services.Media;
using Media.Api.v1.Areas.Media.RequestModels;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Media.Api.v1.RequestModels;

namespace Media.Api.v1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    [ApiController]
    public class FilesController : BaseApiController
    {
        private readonly IMediaService mediaService;

        public FilesController(IMediaService mediaService)
        {
            this.mediaService = mediaService;
        }

        /// <summary>
        /// Gets a file by media id. Provide width and height to resize the image files.
        /// </summary>
        /// <param name="mediaId">The media id to get.</param>
        /// <param name="w">The image max width.</param>
        /// <param name="h">The image max height.</param>
        /// <param name="o">Decrease the image quality to 50 percent and file size.</param>
        /// <param name="extension">The extension to convert an image to: webp, jpg or png.</param>
        /// <returns>The file.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [AllowAnonymous]
        [Route("{mediaId}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof (FileContentResult))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult Get(Guid? mediaId, int? w, int? h, bool o, string? extension)
        {
            var mediaFile = this.mediaService.GetFile(mediaId, w, h, o, extension);

            if (mediaFile != null)
            {
                this.Response.Headers[HeaderNames.CacheControl] = "public,max-age=" + CacheControlConstants.CacheControlMaxAgeSeconds;

                return this.File(mediaFile.File, mediaFile.ContentType, mediaFile.Filename);
            }

            return this.BadRequest();
        }

        /// <param name = "mediaId" > The media id</param>
        /// /// <returns>OK</returns>
        [HttpDelete, MapToApiVersion("1.0")]
        [Route("{mediaId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Delete(Guid? mediaId)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);
            var serviceModel = new DeleteFileServiceModel
            {
                MediaId = mediaId,
                Language = CultureInfo.CurrentCulture.Name,
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new DeleteFileModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);
            if(validationResult.IsValid)
            {
                await this.mediaService.DeleteAsync(serviceModel);

                return this.StatusCode((int)HttpStatusCode.OK);
            }
            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }


        /// <summary>
        /// Uploads a file. The maximum file size is 250MB
        /// </summary>
        /// <param name="model">File contents. Max file size is 250MB</param>
        /// <returns>Created if the file has been uploaded successfully.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Guid))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [RequestSizeLimit(ApiConstants.Request.RequestSizeLimit)]
        public async Task<IActionResult> Create([FromForm] UploadMediaRequestModel model)
        {
            if (model.File == null)
            {
                return this.StatusCode((int)HttpStatusCode.UnprocessableEntity);
            }

            var organisationClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            if (model.Id is not null)
            {
                var serviceModel = new UpdateMediaItemServiceModel
                {
                    Id = Guid.Parse(model.Id),
                    File = model.File,
                    Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GuidHelper.ParseNullable(organisationClaim?.Value),
                    Language = CultureInfo.CurrentCulture.Name
                };

                var validator = new UpdateMediaItemModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);
                if (validationResult.IsValid)
                {
                    var mediaItemId = await this.mediaService.UpdateFileAsync(serviceModel);

                    return this.StatusCode((int)HttpStatusCode.OK, new { Id = mediaItemId });
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            } 
            else
            {
                var serviceModel = new CreateMediaItemServiceModel
                {
                    Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GuidHelper.ParseNullable(organisationClaim?.Value),
                    Language = CultureInfo.CurrentCulture.Name,
                    File = model.File
                };

                var validator = new CreateMediaItemModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);
                if (validationResult.IsValid)
                {
                    var mediaItemId = await this.mediaService.CreateFileAsync(serviceModel);

                    return this.StatusCode((int)HttpStatusCode.Created, new { Id = mediaItemId });
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
        }

        /// <summary>
        /// Uploads a file chunk. The maximum file chunk size is 3 MB.
        /// </summary>
        /// <param name="model">The file chunk contents. The max file chunk size is 3 MB. Please specify the chunk number.</param>
        /// <returns>OK if the file chunk uploaded successfully.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Route("chunks")]
        public async Task<IActionResult> PostChunk([FromForm] UploadMediaRequestModel model)
        {
            var organisationClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new CreateFileChunkServiceModel
            {
                File = model.File,
                ChunkSumber = model.ChunkNumber,
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(organisationClaim?.Value),
                Language = CultureInfo.CurrentCulture.Name
            };

            var validator = new CreateFileChunkModelValidator();

            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                await this.mediaService.CreateFileChunkAsync(serviceModel);

                return this.StatusCode((int)HttpStatusCode.OK);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Completes the upload of file chunks by merging them and saving to the storage.
        /// </summary>
        /// <param name="model">The model that completes upload of media chunks..</param>
        /// <returns>Created if the file has been saved successfully.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Guid))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Route("chunkssavecomplete")]
        public async Task<IActionResult> PostChunksComplete(UploadMediaChunksCompleteRequestModel model)
        {
            var organisationClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            if (model.Id.HasValue)
            {
                var serviceModel = new UpdateMediaItemFromChunksServiceModel
                {
                    Id = model.Id,
                    Filename = model.Filename,
                    Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GuidHelper.ParseNullable(organisationClaim?.Value),
                    Language = CultureInfo.CurrentCulture.Name
                };

                var validator = new UpdateMediaItemFromChunksModelValidator();

                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var mediaItemId = await this.mediaService.UpdateFileFromChunksAsync(serviceModel);

                    return this.StatusCode((int)HttpStatusCode.OK, new { Id = mediaItemId });
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
            else
            {
                var serviceModel = new CreateMediaItemFromChunksServiceModel
                {
                    Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GuidHelper.ParseNullable(organisationClaim?.Value),
                    Language = CultureInfo.CurrentCulture.Name,
                    Filename = model.Filename
                };

                var validator = new CreateMediaItemFromChunksModelValidator();

                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var mediaItemId = await this.mediaService.CreateFileFromChunksAsync(serviceModel);

                    return this.StatusCode((int)HttpStatusCode.Created, new { Id = mediaItemId });
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
        }
    }
}
