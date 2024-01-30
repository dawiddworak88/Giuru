using System;

namespace Client.Api.ServicesModels.Fields
{
    public class FieldOptionServiceModel
    {
        public Guid FieldOptionSetId { get; set; }
        public string Name { get; set; }
        public Guid Value { get; set; }
    }
}
