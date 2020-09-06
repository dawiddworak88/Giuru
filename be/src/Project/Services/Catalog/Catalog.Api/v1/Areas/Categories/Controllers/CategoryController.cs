using Catalog.Api.v1.Areas.Categories.Models;
using Catalog.Api.v1.Areas.Categories.Services;
using Catalog.Api.v1.Areas.Categories.Validators;
using Foundation.ApiExtensions.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.Api.v1.Areas.Categories.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [Authorize]
    [ApiController]
    public class CategoryController : BaseApiController
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet, MapToApiVersion("1.0")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
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

            return this.StatusCode((int)HttpStatusCode.UnprocessableEntity);
        }
    }
}