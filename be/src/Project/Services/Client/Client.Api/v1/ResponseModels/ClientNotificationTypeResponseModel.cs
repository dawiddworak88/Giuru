using System;

namespace Client.Api.v1.ResponseModels
{
    public class ClientNotificationTypeResponseModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LasModifiedDate { get; set; }
    }
}
