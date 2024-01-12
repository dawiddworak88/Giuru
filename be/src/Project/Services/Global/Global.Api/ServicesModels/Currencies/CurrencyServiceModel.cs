using System;

namespace Global.Api.ServicesModels.Currencies
{
    public class CurrencyServiceModel
    {
        public Guid? Id { get; set; }
        public string CurrencyCode { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
