using Foundation.Extensions.Models;
using System;

namespace Global.Api.ServicesModels.Countries
{
    public class DeleteCountryServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
