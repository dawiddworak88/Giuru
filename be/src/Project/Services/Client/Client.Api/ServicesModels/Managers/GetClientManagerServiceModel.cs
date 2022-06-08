using Foundation.Extensions.Models;
using System;

namespace Client.Api.ServicesModels.Managers
{
    public class GetClientManagerServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
