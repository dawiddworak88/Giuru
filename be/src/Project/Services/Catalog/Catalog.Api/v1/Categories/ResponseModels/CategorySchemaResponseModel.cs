﻿using System;

namespace Catalog.Api.v1.Categories.ResponseModels
{
    public class CategorySchemaResponseModel
    {
        public Guid? Id { get; set; }
        public string Schema { get; set; }
        public string UiSchema { get; set; }
        public string Language { get; set; }
    }
}