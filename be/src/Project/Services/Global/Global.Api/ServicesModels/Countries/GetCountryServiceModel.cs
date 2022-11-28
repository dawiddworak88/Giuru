using Foundation.Extensions.Models;
using System;

namespace Global.Api.ServicesModels.Countries
{
    public class GetCountryServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
