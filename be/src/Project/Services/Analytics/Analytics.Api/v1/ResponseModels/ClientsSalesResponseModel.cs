using System;

namespace Analytics.Api.v1.ResponseModels
{
    public class ClientsSalesResponseModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
    }
}
