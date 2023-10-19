using Client.Api.Services.Addresses;
using Foundation.ApiExtensions.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Client.Api.v1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    [ApiController]
    public class ClientAddressesController : BaseApiController
    {
        private readonly IClientAddressesService _clientAddressesService;

        public ClientAddressesController(
            IClientAddressesService clientAddressesService) 
        {
            _clientAddressesService = clientAddressesService;
        }
    }
}
