using Foundation.Extensions.Models;
using System;

namespace Buyer.Web.Shared.ApiRequestModels.Clients
{
    public class PagedClientFieldValuesRequestModel : PagedBaseServiceModel
    {
        public Guid? ClientId { get; set; }
    }
}
