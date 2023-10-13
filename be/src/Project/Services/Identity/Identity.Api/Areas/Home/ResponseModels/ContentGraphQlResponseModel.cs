using Newtonsoft.Json;

namespace Identity.Api.Areas.Home.ResponseModels
{
    public record Attributes(
        [property: JsonProperty("content")] Content Content
    );

    public record Data(
        [property: JsonProperty("id")] string Id,
        [property: JsonProperty("attributes")] Attributes Attributes
    );

    public record Page(
        [property: JsonProperty("data")] Data Data
    );

    public record ContentGraphQlResponseModel(
        [property: JsonProperty("page")] Page Page
    );

    public record Content(
        [property: JsonProperty("title")] string Title,
        [property: JsonProperty("text")] string Text
    );
}
