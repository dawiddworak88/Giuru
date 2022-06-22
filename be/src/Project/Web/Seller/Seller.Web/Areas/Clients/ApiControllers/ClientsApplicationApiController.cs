using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Clients.ApiRequestModels;
using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Areas.Clients.Repositories.Applications;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ApiControllers
{
    [Area("Clients")]
    public class ClientsApplicationApiController : BaseApiController
    {
        private readonly IStringLocalizer<ClientResources> clientLocalizer;
        private readonly IClientApplicationsRepository clientApplicationsRepository;

        public ClientsApplicationApiController(
            IStringLocalizer<ClientResources> clientLocalizer,
            IClientApplicationsRepository clientApplicationsRepository)
        {
            this.clientLocalizer = clientLocalizer;
            this.clientApplicationsRepository = clientApplicationsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int pageIndex, int itemsPerPage)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var groups = await this.clientApplicationsRepository.GetAsync(token, language, searchTerm, pageIndex, itemsPerPage, $"{nameof(ClientApplication.CreatedDate)} desc");

            return this.StatusCode((int)HttpStatusCode.OK, groups);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await this.clientApplicationsRepository.DeleteAsync(token, language, id);

            return this.StatusCode((int)HttpStatusCode.OK, new { Message = this.clientLocalizer.GetString("ApplicationDeletedSuccessfully").Value });
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] ClientApplicationRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var clientApplication = await this.clientApplicationsRepository.SaveAsync(
                token, language, model.Id, model.FirstName, model.LastName, model.ContactJobTitle, model.Email, model.PhoneNumber,
                model.CompanyName, model.CompanyAddress, model.CompanyCountry, model.CompanyCity, model.CompanyRegion, model.CompanyPostalCode);

            return this.StatusCode((int)HttpStatusCode.OK, new { Id = clientApplication, Message = this.clientLocalizer.GetString("ClientApplicationSavedSuccessfully").Value });
        }
    }
}
