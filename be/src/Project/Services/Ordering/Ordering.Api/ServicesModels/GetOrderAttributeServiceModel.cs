﻿using Foundation.Extensions.Models;
using System;

namespace Ordering.Api.ServicesModels
{
    public class GetOrderAttributeServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}