using Foundation.Extensions.Models;
using System;

namespace Client.Api.ServicesModels.Applications
{
    public class GetClientApplicationServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
