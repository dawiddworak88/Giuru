﻿using Foundation.Extensions.Models;
using System;

namespace Global.Api.ServicesModels.Currencies
{
    public class GetCurrencyServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
