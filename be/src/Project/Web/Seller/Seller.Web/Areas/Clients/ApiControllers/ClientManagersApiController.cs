using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Clients.ApiRequestModels;
using Seller.Web.Areas.Clients.Repositories.Managers;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ApiControllers
{
    [Area("Clients")]
    public class ClientManagersApiController : BaseApiController
    {
        private readonly IStringLocalizer<ClientResources> clientLocalizer;
        private readonly IClientManagersRepository clientManagersRepository;

        public ClientManagersApiController(
            IStringLocalizer<ClientResources> clientLocalizer,
            IClientManagersRepository clientManagersRepository)
        {
            this.clientLocalizer = clientLocalizer;
            this.clientManagersRepository = clientManagersRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] ManagerRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var groupId = await this.clientManagersRepository.SaveAsync(token, language, model.Id, model.FirstName, model.LastName, model.Email, model.PhoneNumber);

            return this.StatusCode((int)HttpStatusCode.OK, new { Id = groupId, Message = this.clientLocalizer.GetString("ManagerSavedSuccessfully").Value });
        }
    }
}
