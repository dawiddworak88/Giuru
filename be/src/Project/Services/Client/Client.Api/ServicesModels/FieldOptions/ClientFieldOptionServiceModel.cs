using System;

namespace Client.Api.ServicesModels.FieldOptions
{
    public class ClientFieldOptionServiceModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
