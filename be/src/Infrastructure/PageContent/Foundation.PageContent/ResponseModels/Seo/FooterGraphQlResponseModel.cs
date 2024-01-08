using Newtonsoft.Json;
using System.Collections.Generic;

namespace Buyer.Web.Shared.GraphQlResponseModels
{
    public record FooterGraphQlResponseModel(
        [property: JsonProperty("globalConfiguration")] FooterComponent Component
    );

    public record FooterComponent(
        [property: JsonProperty("data")] FooterData Data
    );

    public record FooterData(
        [property: JsonProperty("id")] string Id,
        [property: JsonProperty("attributes")] FooterAttributes Attributes
    );

    public record FooterAttributes(
        [property: JsonProperty("footer")] Footer Footer
    );

    public record Footer(
        [property: JsonProperty("copyright")] string Copyright,
        [property: JsonProperty("links")] IEnumerable<LinkGraphQlResponseModel> Links
    );
}
