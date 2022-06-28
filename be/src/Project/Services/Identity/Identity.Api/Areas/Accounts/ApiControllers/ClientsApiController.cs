using Feature.Account;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Identity.Api.Areas.Accounts.ApiRequestModels;
using Identity.Api.Areas.Accounts.Repositories.Clients;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Identity.Api.Areas.Accounts.ApiControllers
{
    [Area("Accounts")]
    public class ClientsApiController : BaseApiController
    {
        private readonly IStringLocalizer<AccountResources> accountLocalizer;
        private readonly IClientsRepository clientsRepository;

        public ClientsApiController(
            IStringLocalizer<AccountResources> accountLocalizer,
            IClientsRepository clientsRepository)
        {
            this.accountLocalizer = accountLocalizer;
            this.clientsRepository = clientsRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Application([FromBody] ClientApplicationRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await this.clientsRepository.CreateClientApplicationAsync(
                token, language, model.FirstName, model.LastName, model.ContactJobTitle, model.Email, model.PhoneNumber, model.CompanyName, 
                model.CompanyAddress, model.CompanyCountry, model.CompanyCity, model.CompanyRegion, model.CompanyPostalCode);

            return this.StatusCode((int)HttpStatusCode.OK, new { Message = this.accountLocalizer.GetString("SuccessfullyClientApply").Value });
        }
    }
}
