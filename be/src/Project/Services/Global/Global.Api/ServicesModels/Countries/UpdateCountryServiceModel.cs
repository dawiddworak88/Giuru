using Foundation.Extensions.Models;
using System;

namespace Global.Api.ServicesModels.Countries
{
    public class UpdateCountryServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set;  }
    }
}
