using Catalog.Api.Services.Products;
using Catalog.Api.ServicesModels.Products;
using Catalog.Api.v1.Products.RequestModels;
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
using System.Threading.Tasks;

namespace Catalog.Api.v1.Products.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ProductsSearchIndexController : BaseApiController
    {
        private readonly IProductsService productsService;

        public ProductsSearchIndexController(IProductsService productsService)
        {
            this.productsService = productsService;
        }

        /// <summary>
        /// Triggers catalog index rebuild.
        /// </summary>
        /// <returns>Accepted if the catalog index rebuild has been started correctly.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Post()
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.OrganisationIdClaim);

            var serviceModel = new RebuildCatalogIndexServiceModel
            {
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                Language = CultureInfo.CurrentCulture.Name
            };

            var validator = new RebuildCatalogIndexModelValidator();

            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                await this.productsService.TriggerCatalogIndexRebuildAsync(serviceModel);

                return this.StatusCode((int)HttpStatusCode.Accepted);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }
    }
}
