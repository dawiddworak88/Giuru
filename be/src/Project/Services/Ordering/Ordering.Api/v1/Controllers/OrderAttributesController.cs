using Foundation.ApiExtensions.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ordering.Api.Services.OrderAttributes;

namespace Ordering.Api.v1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    [ApiController]
    public class OrderAttributesController : BaseApiController
    {
        private readonly IOrderAttributesService _orderAttributesService;

        public OrderAttributesController(IOrderAttributesService orderAttributesService)
        {
            _orderAttributesService = orderAttributesService;
        }


    }
}
