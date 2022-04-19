using Foundation.Extensions.Models;
using System;

namespace Client.Api.ServicesModels.Groups
{
    public class GetGroupServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
