using DownloadCenter.Api.Services.DownloadCenter;
using DownloadCenter.Api.ServicesModels.DownloadCenter;
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

        private readonly IDownloadCenterService downloadsService;

        public DownloadCenterController(
            IDownloadCenterService downloadsService)
        {
            this.downloadsService = downloadsService;
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
                var downloadCategory = await this.downloadsService.GetAsync(serviceModel);

                if (downloadCategory is not null)
                {
                    var response = new DownloadCategoriesResponseModel
                    {
                        Id = downloadCategory.Id,
                        CategoryName = downloadCategory.CategoryName,
                        Categories = downloadCategory.Categories.OrEmptyIfNull().Select(x => new DownloadCategoryResponseModel
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
        /// Gets list of downloads.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <param name="orderBy">The optional order by.</param>
        /// <returns>The list of downloads.</returns>
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
                var categories = await this.downloadsService.GetAsync(serviceModel);

                if (categories is not null)
                {
                    var response = new PagedResults<IEnumerable<DownloadResponseModel>>(categories.Total, categories.PageSize)
                    {
                        Data = categories.Data.OrEmptyIfNull().Select(x => new DownloadResponseModel
                        {
                            Id = x.Id,
                            CategoryId = x.CategoryId,
                            CategoryName = x.CategoryName,
                            Categories = x.Categories.OrEmptyIfNull().Select(y => new DownloadCategoryResponseModel
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
    }
}
