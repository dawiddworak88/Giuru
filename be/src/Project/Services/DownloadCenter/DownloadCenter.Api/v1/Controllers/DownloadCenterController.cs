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
    [AllowAnonymous]
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
        /// Get download category by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The download category.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("category/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(DownloadCategoriesResponseModel))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Get(Guid? id)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetDownloadCategoryServiceModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name,
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new GetDownloadCategoryModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var downloadCategory = await this.downloadCenterService.GetDownloadCenterCategoryAsync(serviceModel);

                if (downloadCategory is not null)
                {
                    var response = new DownloadCategoriesResponseModel
                    {
                        Id = downloadCategory.Id,
                        CategoryName = downloadCategory.CategoryName,
                        Categories = downloadCategory.Categories.OrEmptyIfNull().Select(x => new DownloadCenterCategoryResponseModel
                        {
                            Id = x.Id,
                            Name = x.Name
                        }),
                        LastModifiedDate = downloadCategory.LastModifiedDate,
                        CreatedDate = downloadCategory.CreatedDate
                    };

                    return this.StatusCode((int)HttpStatusCode.OK, response);
                }
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Gets list of download center items.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <param name="orderBy">The optional order by.</param>
        /// <returns>The list of download center items.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PagedResults<IEnumerable<CategoryResponseModel>>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Get(string searchTerm, int pageIndex, int itemsPerPage, string orderBy)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetDownloadCenterServiceModel
            {
                SearchTerm = searchTerm,
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage,
                OrderBy = orderBy,
                Language = CultureInfo.CurrentCulture.Name,
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new GetDownloadCenterModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var categories = await this.downloadCenterService.GetAsync(serviceModel);

                if (categories is not null)
                {
                    var response = new PagedResults<IEnumerable<DownloadCenterResponseModel>>(categories.Total, categories.PageSize)
                    {
                        Data = categories.Data.OrEmptyIfNull().Select(x => new DownloadCenterResponseModel
                        {
                            Id = x.Id,
                            CategoryId = x.CategoryId,
                            CategoryName = x.CategoryName,
                            Categories = x.Categories.OrEmptyIfNull().Select(y => new DownloadCenterCategoryResponseModel
                            {
                                Id = y.Id,
                                Name = y.Name
                            }),
                            Order = x.Order,
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
        /// Delete download center item by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>OK.</returns>
        [HttpDelete, MapToApiVersion("1.0")]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
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
        /// Get download center item by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The download center item.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(DownloadCenterItemResponseModel))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> GetItem(Guid? id)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetDownloadCenterItemServiceModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name,
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new GetDownloadCenterItemModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var downloadCenterItem = await this.downloadCenterService.GetAsync(serviceModel);

                if (downloadCenterItem is not null)
                {
                    var response = new DownloadCenterItemResponseModel
                    {
                        Id = downloadCenterItem.Id,
                        CategoryId = downloadCenterItem.CategoryId,
                        CategoryName = downloadCenterItem.CategoryName,
                        Order = downloadCenterItem.Order,
                        LastModifiedDate = downloadCenterItem.LastModifiedDate,
                        CreatedDate = downloadCenterItem.CreatedDate
                    };

                    return this.StatusCode((int)HttpStatusCode.OK, response);
                }
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Creates or updates download center item (if id is set).
        /// </summary>
        /// <param name="request">The model.</param>
        /// <returns>The download center item id.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Save(DownloadCenterItemRequestModel request)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            if (request.Id.HasValue)
            {
                var serviceModel = new UpdateDownloadCenterItemServiceModel
                {
                    Id = request.Id,
                    CategoryId = request.CategoryId,
                    Order = request.Order,
                    Language = CultureInfo.CurrentCulture.Name,
                    Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
                };

                var validator = new UpdateDownloadCenterItemModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var downloadCenterItemId = await this.downloadCenterService.UpdateAsync(serviceModel);

                    return this.StatusCode((int)HttpStatusCode.OK, new { Id = downloadCenterItemId });
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
            else
            {
                var serviceModel = new CreateDownloadCenterItemServiceModel
                {
                    CategoryId = request.CategoryId,
                    Order = request.Order,
                    Language = CultureInfo.CurrentCulture.Name,
                    Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
                };

                var validator = new CreateDownloadCenterItemModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var downloadCenterItemId = await this.downloadCenterService.CreateAsync(serviceModel);

                    return this.StatusCode((int)HttpStatusCode.OK, new { Id = downloadCenterItemId });
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
        }
    }
}
