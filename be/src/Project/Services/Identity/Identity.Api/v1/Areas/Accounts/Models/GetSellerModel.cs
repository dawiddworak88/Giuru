using Foundation.Extensions.Models;
using System;

namespace Identity.Api.v1.Areas.Accounts.Models
{
    public class GetSellerModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
