using System;

namespace Global.Api.ServicesModels.Countries
{
    public class CountryServiceModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
