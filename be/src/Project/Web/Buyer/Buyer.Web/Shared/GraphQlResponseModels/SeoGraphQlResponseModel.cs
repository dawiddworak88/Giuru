﻿using Newtonsoft.Json;

namespace Buyer.Web.Shared.GraphQlResponseModels
{
    public record Attributes(
        [property: JsonProperty("seo")] Seo Seo
    );

    public record Data(
        [property: JsonProperty("id")] string Id,
        [property: JsonProperty("attributes")] Attributes Attributes
    );

    public record HomePage(
        [property: JsonProperty("data")] Data Data
    );

    public record SeoGraphQlResponseModel(
        [property: JsonProperty("homePage")] HomePage HomePage
    );

    public record Seo(
        [property: JsonProperty("metaTitle")] string MetaTitle,
        [property: JsonProperty("metaDescription")] string MetaDescription
    );
}