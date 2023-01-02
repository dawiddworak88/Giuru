using System;

namespace Global.Api.v1.ResponseModels
{
    public class CountryResponseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
