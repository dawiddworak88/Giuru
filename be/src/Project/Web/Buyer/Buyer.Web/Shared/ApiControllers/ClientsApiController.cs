using Buyer.Web.Shared.ApiRequestModels.Application;
using Buyer.Web.Shared.Repositories.Clients;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.ApiControllers
{
    [Area("Home")]
    [AllowAnonymous]
    public class ClientsApiController : BaseApiController
    {
        private readonly IClientsRepository clientsRepository;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;

        public ClientsApiController(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IClientsRepository clientsRepository)
        {
            this.clientsRepository = clientsRepository;
            this.globalLocalizer = globalLocalizer;
        }

        [HttpPost]
        public async Task<IActionResult> Application([FromBody] ClientApplicationRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await this.clientsRepository.CreateClientApplicationAsync(
                token, language, model.FirstName, model.LastName, model.ContactJobTitle, model.Email, model.PhoneNumber, model.CompanyName,
                model.CompanyAddress, model.CompanyCountry, model.CompanyCity, model.CompanyRegion, model.CompanyPostalCode);

            return this.StatusCode((int)HttpStatusCode.OK, new { Message = this.globalLocalizer.GetString("SuccessfullyClientApply").Value });
        }
    }
}
