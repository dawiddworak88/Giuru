using Foundation.Extensions.Models;
using System;

namespace Client.Api.ServicesModels.Groups
{
    public class UpdateClientGroupServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
    }
}
