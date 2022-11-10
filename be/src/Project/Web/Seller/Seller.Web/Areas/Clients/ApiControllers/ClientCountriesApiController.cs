using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Clients.ApiRequestModels;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using System;
using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Areas.Clients.Repositories.Countries;

namespace Seller.Web.Areas.Clients.ApiControllers
{
    [Area("Clients")]
    public class ClientCountriesApiController : BaseApiController
    {
        private readonly IStringLocalizer<ClientResources> clientLocalizer;
        private readonly IClientCountriesRepository clientCountriesRepository;

        public ClientCountriesApiController(
            IStringLocalizer<ClientResources> clientLocalizer,
            IClientCountriesRepository clientCountriesRepository)
        {
            this.clientLocalizer = clientLocalizer;
            this.clientCountriesRepository = clientCountriesRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] RoleRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await this.clientCountriesRepository.SaveAsync(token, language, model.Id, model.Name);

            return this.StatusCode((int)HttpStatusCode.OK, new { Message = this.clientLocalizer.GetString("CountrySavedSuccessfully").Value });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await this.clientCountriesRepository.DeleteAsync(token, language, id);

            return this.StatusCode((int)HttpStatusCode.OK, new { Message = this.clientLocalizer.GetString("CountryDeletedSuccessfully").Value });
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int pageIndex, int itemsPerPage)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var roles = await this.clientCountriesRepository.GetAsync(
                token, language, searchTerm, pageIndex, itemsPerPage, $"{nameof(ClientCountry.CreatedDate)} desc");

            return this.StatusCode((int)HttpStatusCode.OK, roles);
        }
    }
}
