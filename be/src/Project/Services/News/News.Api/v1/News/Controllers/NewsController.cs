using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Controllers;
using Foundation.Extensions.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.Helpers;
using Foundation.GenericRepository.Paginations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using News.Api.Services.News;
using News.Api.ServicesModels.News;
using News.Api.v1.Categories.RequestModels;
using News.Api.v1.News.ResponseModels;
using News.Api.Validators.News;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace News.Api.v1.News.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    [ApiController]
    public class NewsController : BaseApiController
    {
        private readonly INewsService newsService;

        public NewsController(
            INewsService newsService)
        {
            this.newsService = newsService;
        }

        /// <summary>
        /// Get list of news item files.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <param name="orderBy">The optional order by.</param>
        /// <param name="id">The news item id.</param>
        /// <returns>The list of news item files.</returns>
        [HttpGet("files/{id}"), MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PagedResults<IEnumerable<NewsItemFileResponseModel>>))]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Files(Guid? id, string searchTerm, int? pageIndex, int? itemsPerPage, string orderBy)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetNewsItemFilesServiceModel
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

            var validator = new GetNewsItemFilesModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var files = await this.newsService.GetFilesAsync(serviceModel);

                if (files is not null)
                {
                    var response = new PagedResults<IEnumerable<NewsItemFileResponseModel>>(files.Total, files.PageSize)
                    {
                        Data = files.Data.OrEmptyIfNull().Select(x => new NewsItemFileResponseModel
                        {
                            Id = x.Id,
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
        /// Creates or updates news (if news id is set).
        /// </summary>
        /// <param name="request">The model.</param>
        /// <returns>The news id.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Save(NewsRequestModel request) 
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            if (request.Id.HasValue)
            {
                var serviceModel = new UpdateNewsItemServiceModel
                {
                    Id = request.Id,
                    Title = request.Title,
                    Description = request.Description,
                    Content = request.Content,
                    IsPublished = request.IsPublished,
                    CategoryId = request.CategoryId,
                    PreviewImageId = request.PreviewImageId,
                    ThumbnailImageId = request.ThumbnailImageId,
                    Files = request.Files,
                    Language = CultureInfo.CurrentCulture.Name,
                    Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
                };

                var validator = new UpdateNewsItemModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var newsId = await this.newsService.UpdateAsync(serviceModel);

                    return this.StatusCode((int)HttpStatusCode.OK, new { Id = newsId });
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            } 
            else
            {
                var serviceModel = new CreateNewsItemServiceModel
                {
                    Title = request.Title,
                    Description = request.Description,
                    Content = request.Content,
                    IsPublished = request.IsPublished,
                    CategoryId = request.CategoryId,
                    ThumbnailImageId = request.ThumbnailImageId,
                    PreviewImageId = request.PreviewImageId,
                    Files = request.Files,
                    Language = CultureInfo.CurrentCulture.Name,
                    Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
                };

                var validator = new CreateNewsItemModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);


                if (validationResult.IsValid)
                {
                    var newsId = await this.newsService.CreateAsync(serviceModel);

                    return this.StatusCode((int)HttpStatusCode.OK, new { Id = newsId });
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
        }

        /// <summary>
        /// Gets list of news.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <param name="orderBy">The optional order by.</param>
        /// <returns>The list of news.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PagedResults<IEnumerable<NewsItemResponseModel>>))]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Get(string searchTerm, int? pageIndex, int? itemsPerPage, string orderBy)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);
            var serviceModel = new GetNewsItemsServiceModel
            {
                SearchTerm = searchTerm,
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage,
                OrderBy = orderBy,
                Language = CultureInfo.CurrentCulture.Name,
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

             var newsItems = await this.newsService.GetAsync(serviceModel);

             if (newsItems is not null)
             {
                var response = new PagedResults<IEnumerable<NewsItemResponseModel>>(newsItems.Total, newsItems.PageSize)
                {
                    Data = newsItems.Data.OrEmptyIfNull().Select(x => new NewsItemResponseModel
                    {
                        Id = x.Id,
                        ThumbnailImageId = x.ThumbnailImageId,
                        PreviewImageId = x.PreviewImageId,
                        CategoryId = x.CategoryId,
                        CategoryName = x.CategoryName,
                        Title = x.Title,
                        Description = x.Description,
                        Content = x.Content,
                        IsPublished = x.IsPublished,
                        Files = x.Files,
                        LastModifiedDate = x.LastModifiedDate,
                        CreatedDate = x.CreatedDate
                    })
                };

                return this.StatusCode((int)HttpStatusCode.OK, response);
            }

            throw new CustomException("", (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Gets news by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The news item.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("{id}")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(NewsItemResponseModel))]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Get(Guid? id)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);
            var serviceModel = new GetNewsItemServiceModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name,
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new GetNewsItemModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);
            if (validationResult.IsValid)
            {
                var newsItem = await this.newsService.GetAsync(serviceModel);

                if (newsItem is not null)
                {
                    var response = new NewsItemResponseModel
                    {
                        Id = newsItem.Id,
                        ThumbnailImageId = newsItem.ThumbnailImageId,
                        PreviewImageId = newsItem.PreviewImageId,
                        CategoryId = newsItem.CategoryId,
                        CategoryName = newsItem.CategoryName,
                        Title = newsItem.Title,
                        Description = newsItem.Description,
                        Content = newsItem.Content,
                        IsPublished = newsItem.IsPublished,
                        Files = newsItem.Files,
                        LastModifiedDate = newsItem.LastModifiedDate,
                        CreatedDate = newsItem.CreatedDate
                    };

                    return this.StatusCode((int)HttpStatusCode.OK, response);
                }

                return this.StatusCode((int)HttpStatusCode.NoContent);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Delete category by id.
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
            var serviceModel = new DeleteNewsItemServiceModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name,
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new DeleteNewsItemModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                await this.newsService.DeleteAsync(serviceModel);

                return this.StatusCode((int)HttpStatusCode.OK);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }
    }
}
