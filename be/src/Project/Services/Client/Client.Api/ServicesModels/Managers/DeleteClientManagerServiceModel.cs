using Foundation.Extensions.Models;
using System;

namespace Client.Api.ServicesModels.Managers
{
    public class DeleteClientManagerServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
