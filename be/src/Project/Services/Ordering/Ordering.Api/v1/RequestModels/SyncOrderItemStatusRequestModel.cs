﻿using System;

namespace Ordering.Api.v1.RequestModels
{
    public class SyncOrderItemStatusRequestModel
    {
        public Guid Id { get; set; }
        public Guid StatusId { get; set; }
        public string StatusChangeComment { get; set; }
        public string Language { get; set; }
    }
}
