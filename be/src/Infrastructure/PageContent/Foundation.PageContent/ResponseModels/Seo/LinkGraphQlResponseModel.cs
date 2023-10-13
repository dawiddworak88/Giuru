using Newtonsoft.Json;

namespace Buyer.Web.Shared.GraphQlResponseModels
{
    public record LinkGraphQlResponseModel(
        [property: JsonProperty("href")] string Href,
        [property: JsonProperty("label")] string Label,
        [property: JsonProperty("target")] string Target,
        [property: JsonProperty("isExternal")] bool IsExternal
    );
}
