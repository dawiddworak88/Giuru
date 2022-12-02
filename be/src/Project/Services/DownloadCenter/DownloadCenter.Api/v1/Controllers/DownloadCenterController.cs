using DownloadCenter.Api.Services.DownloadCenter;
using DownloadCenter.Api.ServicesModels.DownloadCenter;
using DownloadCenter.Api.v1.RequestModel;
using DownloadCenter.Api.v1.ResponseModel;
using DownloadCenter.Api.Validators.DownloadCenter;
using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Controllers;
using Foundation.Extensions.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.Helpers;
using Foundation.GenericRepository.Paginations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DownloadCenter.Api.v1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    [ApiController]
    public class DownloadCenterController : BaseApiController
    {
        private readonly IDownloadCenterService downloadCenterService;

        public DownloadCenterController(
            IDownloadCenterService downloadCenterService)
        {
            this.downloadCenterService = downloadCenterService;
        }

        /// <summary>
        /// Get list of download center category files.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <param name="orderBy">The optional order by.</param>
        /// <param name="id">The category id.</param>
        /// <returns>The list of download center category files.</returns>
        [HttpGet("categories/files/{id}"), MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PagedResults<IEnumerable<DownloadCenterCategoryFileResponseModel>>))]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> CategoryFiles(Guid? id, string searchTerm, int? pageIndex, int? itemsPerPage, string orderBy)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetDownloadCenterCategoryFilesServiceModel
            {
                Id = id,
                SearchTerm = searchTerm,
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage,
                OrderBy = orderBy,
                Language = CultureInfo.CurrentCulture.Name,
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new GetDownloadCenterCategoryFilesModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var downloadCenterCategoryFiles = await this.downloadCenterService.GetDownloadCenterCategoryFilesAsync(serviceModel);

                if (downloadCenterCategoryFiles is not null)
                {
                    var response = new PagedResults<IEnumerable<DownloadCenterCategoryFileResponseModel>>(downloadCenterCategoryFiles.Total, downloadCenterCategoryFiles.PageSize)
                    {
                        Data = downloadCenterCategoryFiles.Data.OrEmptyIfNull().Select(x => new DownloadCenterCategoryFileResponseModel
                        {
                            Id = x.Id,
                            Filename = x.Filename,
                            LastModifiedDate = x.LastModifiedDate,
                            CreatedDate = x.CreatedDate
                        })
                    };

                    return this.StatusCode((int)HttpStatusCode.OK, response);
                }
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Get download center by category id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The download center category.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("categories/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(DownloadCenterCategoryResponseModel))]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Get(Guid? id)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetDownloadCenterCategoryServiceModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name,
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new GetDownloadCenterCategoryModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var downloadCenterCategory = await this.downloadCenterService.GetDownloadCenterCategoryAsync(serviceModel);

                if (downloadCenterCategory is not null)
                {
                    var response = new DownloadCenterCategoryResponseModel
                    {
                        Id = downloadCenterCategory.Id,
                        ParentCategoryId = downloadCenterCategory.ParentCategoryId,
                        ParentCategoryName = downloadCenterCategory.ParentCategoryName,
                        CategoryName = downloadCenterCategory.CategoryName,
                        Subcategories = downloadCenterCategory.Subcategories.OrEmptyIfNull().Select(x => new DownloadCenterSubcategoryResponseModel
                        {
                            Id = x.Id,
                            Name = x.Name
                        }),
                        Files = downloadCenterCategory.Files,
                        LastModifiedDate = downloadCenterCategory.LastModifiedDate,
                        CreatedDate = downloadCenterCategory.CreatedDate
                    };

                    return this.StatusCode((int)HttpStatusCode.OK, response);
                }
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Get list of download center.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <param name="orderBy">The optional order by.</param>
        /// <returns>The list of download center.</returns>
        [HttpGet("categories"), MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PagedResults<IEnumerable<DownloadCenterCategoryItemResponseModel>>))]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> GetDownloadCenter(string searchTerm, int? pageIndex, int? itemsPerPage, string orderBy)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetDownloadCenterItemsServiceModel
            {
                SearchTerm = searchTerm,
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage,
                OrderBy = orderBy,
                Language = CultureInfo.CurrentCulture.Name,
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new GetDownloadCenterItemsModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var downloadCenterFiles = await this.downloadCenterService.GetAsync(serviceModel);

                if (downloadCenterFiles is not null)
                {
                    var response = new PagedResults<IEnumerable<DownloadCenterCategoryItemResponseModel>>(downloadCenterFiles.Total, downloadCenterFiles.PageSize)
                    {
                        Data = downloadCenterFiles.Data.OrEmptyIfNull().Select(x => new DownloadCenterCategoryItemResponseModel
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Subcategories = x.Subcategories.OrEmptyIfNull().Select(y => new DownloadCenterSubcategoryResponseModel
                            {
                                Id = y.Id,
                                Name = y.Name
                            }),
                            LastModifiedDate = x.LastModifiedDate,
                            CreatedDate = x.CreatedDate
                        })
                    };

                    return this.StatusCode((int)HttpStatusCode.OK, response);
                }
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Get list of download center files.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <param name="orderBy">The optional order by.</param>
        /// <returns>The list of download center files.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PagedResults<IEnumerable<DownloadCenterItemResponseModel>>))]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Get(string searchTerm, int? pageIndex, int? itemsPerPage, string orderBy)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetDownloadCenterFilesServiceModel
            {
                SearchTerm = searchTerm,
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage,
                OrderBy = orderBy,
                Language = CultureInfo.CurrentCulture.Name,
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new GetDownloadCenterFilesModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var downloadCenterFiles = await this.downloadCenterService.GetAsync(serviceModel);

                if (downloadCenterFiles is not null)
                {
                    var response = new PagedResults<IEnumerable<DownloadCenterItemResponseModel>>(downloadCenterFiles.Total, downloadCenterFiles.PageSize)
                    {
                        Data = downloadCenterFiles.Data.OrEmptyIfNull().Select(x => new DownloadCenterItemResponseModel
                        {
                            Id = x.Id,
                            Filename = x.Filename,
                            Categories = x.Categories,
                            LastModifiedDate = x.LastModifiedDate,
                            CreatedDate = x.CreatedDate
                        })
                    };

                    return this.StatusCode((int)HttpStatusCode.OK, response);
                }
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Delete download center file by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>OK.</returns>
        [HttpDelete, MapToApiVersion("1.0")]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new DeleteDownloadCenterItemServiceModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name,
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new DeleteDownloadCenterItemModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                await this.downloadCenterService.DeleteAsync(serviceModel);

                return this.StatusCode((int)HttpStatusCode.OK);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Get download center file by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The download center file.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(DownloadCenterItemFileResponseModel))]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> GetDownloadCenterFile(Guid? id)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetDownloadCenterFileServiceModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name,
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new GetDownloadCenterFileModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var downloadCenterFile = await this.downloadCenterService.GetAsync(serviceModel);

                if (downloadCenterFile is not null)
                {
                    var response = new DownloadCenterItemFileResponseModel
                    {
                        Id = downloadCenterFile.Id,
                        CategoriesIds = downloadCenterFile.CategoriesIds,
                        LastModifiedDate = downloadCenterFile.LastModifiedDate,
                        CreatedDate = downloadCenterFile.CreatedDate
                    };

                    return this.StatusCode((int)HttpStatusCode.OK, response);
                }
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Creates or updates download center files (if id is set).
        /// </summary>
        /// <param name="request">The model.</param>
        /// <returns>The download center file id.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Save(DownloadCenterItemRequestModel request)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            if (request.Id.HasValue)
            {
                var serviceModel = new UpdateDownloadCenterItemServiceModel
                {
                    Id = request.Id,
                    CategoriesIds = request.CategoriesIds,
                    Files = request.Files.Select(x => new DownloadCenterFileServiceModel
                    {
                        Id = x.Id,
                        Filename = x.Name
                    }),
                    Language = CultureInfo.CurrentCulture.Name,
                    Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
                };

                var validator = new UpdateDownloadCenterItemModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var downloadCenterFileId = await this.downloadCenterService.UpdateAsync(serviceModel);

                    return this.StatusCode((int)HttpStatusCode.OK, new { Id = downloadCenterFileId });
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
            else
            {
                var serviceModel = new CreateDownloadCenterItemServiceModel
                {
                    CategoriesIds = request.CategoriesIds,
                    Files = request.Files.Select(x => new DownloadCenterFileServiceModel
                    {
                        Id = x.Id,
                        Filename = x.Name
                    }),
                    Language = CultureInfo.CurrentCulture.Name,
                    Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
                };

                var validator = new CreateDownloadCenterItemModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var downloadCenterFileId = await this.downloadCenterService.CreateAsync(serviceModel);

                    return this.StatusCode((int)HttpStatusCode.OK, new { Id = downloadCenterFileId });
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
        }
    }
}
