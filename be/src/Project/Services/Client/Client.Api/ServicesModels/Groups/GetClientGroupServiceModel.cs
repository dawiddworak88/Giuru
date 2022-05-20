using Foundation.Extensions.Models;
using System;

namespace Client.Api.ServicesModels.Groups
{
    public class GetClientGroupServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
