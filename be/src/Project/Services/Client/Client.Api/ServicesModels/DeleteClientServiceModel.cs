using Foundation.Extensions.Models;
using System;

namespace Client.Api.ServicesModels
{
    public class DeleteClientServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
