using Catalog.Api.Services.Products;
using Catalog.Api.ServicesModels.Products;
using Catalog.Api.Validators.Products;
using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Controllers;
using Foundation.Extensions.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;

namespace Catalog.Api.v1.Products.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ProductsSearchIndexController : BaseApiController
    {
        private readonly IProductsService _productsService;

        public ProductsSearchIndexController(IProductsService productsService)
        {
            _productsService = productsService;
        }

        /// <summary>
        /// Triggers catalog index rebuild.
        /// </summary>
        /// <returns>Accepted if the catalog index rebuild has been started correctly.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public IActionResult Post()
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new RebuildCatalogIndexServiceModel
            {
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                Language = CultureInfo.CurrentCulture.Name
            };

            var validator = new RebuildCatalogIndexModelValidator();

            var validationResult = validator.Validate(serviceModel);

            if (validationResult.IsValid)
            {
                _productsService.TriggerCatalogIndexRebuild(serviceModel);

                return this.StatusCode((int)HttpStatusCode.Accepted);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }
    }
}
