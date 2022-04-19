using Foundation.Extensions.Models;
using System;

namespace Client.Api.ServicesModels.Groups
{
    public class UpdateGroupServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
    }
}
