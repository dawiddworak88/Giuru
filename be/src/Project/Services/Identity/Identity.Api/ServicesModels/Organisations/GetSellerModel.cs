using Foundation.Extensions.Models;
using System;

namespace Identity.Api.ServicesModels.Organisations
{
    public class GetSellerModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
