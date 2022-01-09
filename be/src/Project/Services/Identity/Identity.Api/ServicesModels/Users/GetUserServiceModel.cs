using Foundation.Extensions.Models;
using System;

namespace Identity.Api.ServicesModels.Users
{
    public class GetUserServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
