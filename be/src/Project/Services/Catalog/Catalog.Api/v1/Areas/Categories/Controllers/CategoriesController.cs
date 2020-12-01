using Catalog.Api.v1.Areas.Categories.Models;
using Catalog.Api.v1.Areas.Categories.Services;
using Catalog.Api.v1.Areas.Categories.Validators;
using Foundation.ApiExtensions.Controllers;
using Foundation.Extensions.Definitions;
using Foundation.Extensions.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.Api.v1.Areas.Categories.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [Authorize]
    [ApiController]
    public class CategoriesController : BaseApiController
    {
        private readonly ICategoryService categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        /// <summary>
        /// Gets list of categories.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="level">The level.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <returns>The list of categories.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> Get(string language, string searchTerm, int? level, int pageIndex, int itemsPerPage)
        {
            var serviceModel = new GetCategoriesModel
            {
                Level = level,
                Language = language,
                SearchTerm = searchTerm,
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage
            };

            var validator = new GetCategoriesModelValidator();

            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var categories = await this.categoryService.GetAsync(serviceModel);

                return categories?.Data != null && categories.Data.Any()
                    ? this.StatusCode((int)HttpStatusCode.OK, categories)
                    : (IActionResult)this.StatusCode((int)HttpStatusCode.NotFound);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Gets category by id.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <param name="id">The id.</param>
        /// <returns>The category.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> Get(string language, Guid? id)
        {
            var serviceModel = new GetCategoryModel
            {
                Id = id,
                Language = language
            };

            var validator = new GetCategoryModelValidator();

            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var category = await this.categoryService.GetAsync(serviceModel);

                return category != null ? this.StatusCode((int)HttpStatusCode.OK, category) : (IActionResult)this.StatusCode((int)HttpStatusCode.NotFound);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Delete category by id.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <param name="id">The id.</param>
        /// <returns>The category.</returns>
        [HttpDelete, MapToApiVersion("1.0")]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> Delete(string language, Guid? id)
        {
            var serviceModel = new DeleteCategoryModel
            {
                Id = id,
                Language = language
            };

            var validator = new DeleteCategoryModelValidator();

            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                await this.categoryService.DeleteAsync(serviceModel);

                return this.StatusCode((int)HttpStatusCode.OK);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }
    }
}
