﻿using System;

namespace Client.Api.ServicesModels.Groups
{
    public class ClientGroupServiceModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
