using Newtonsoft.Json;

namespace Foundation.PageContent.ResponseModels.Seo
{
    public record Attributes(
        [property: JsonProperty("seo")] Seo Seo
    );

    public record Data(
        [property: JsonProperty("id")] string Id,
        [property: JsonProperty("attributes")] Attributes Attributes
    );

    public record Page(
        [property: JsonProperty("data")] Data Data
    );

    public record SeoGraphQlResponseModel(
        [property: JsonProperty("page")] Page Page
    );

    public record Seo(
        [property: JsonProperty("metaTitle")] string MetaTitle,
        [property: JsonProperty("metaDescription")] string MetaDescription
    );
}
