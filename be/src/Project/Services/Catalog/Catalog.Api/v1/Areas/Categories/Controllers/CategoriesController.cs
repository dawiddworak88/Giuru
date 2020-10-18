using Catalog.Api.v1.Areas.Categories.Models;
using Catalog.Api.v1.Areas.Categories.Services;
using Catalog.Api.v1.Areas.Categories.Validators;
using Foundation.ApiExtensions.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet, MapToApiVersion("1.0")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> Get(string language)
        {
            var serviceModel = new GetCategoriesModel
            {
                Language = language
            };

            var validator = new GetCategoriesModelValidator();

            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var categories = await this.categoryService.GetAsync(serviceModel);

                return categories != null && categories.Any()
                    ? this.StatusCode((int)HttpStatusCode.OK, categories)
                    : (IActionResult)this.StatusCode((int)HttpStatusCode.NotFound);
            }

            return this.StatusCode((int)HttpStatusCode.UnprocessableEntity);
        }
    }
}
