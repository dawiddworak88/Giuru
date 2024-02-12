using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Identity.Api.Areas.Home.ResponseModels.Policy
{
    public record Attributes(
    [property: JsonProperty("policy")] Policy Policy
);

    public record Data(
        [property: JsonProperty("id")] string Id,
        [property: JsonProperty("attributes")] Attributes Attributes
    );

    public record Page(
        [property: JsonProperty("data")] Data Data
    );

    public record PolicyGraphQlResponseModel(
        [property: JsonProperty("privacyPolicyPage")] Page Page
    );

    public record Policy(
        [property: JsonProperty("title")] string Title,
        [property: JsonProperty("description")] string Description,
        [property: JsonProperty("accordion")] Accordion Accordion
    );

    public record Accordion(
        [property: JsonProperty("accordionItems")] IEnumerable<AccordionItem> AccordionItems
        );

    public record AccordionItem(
        [property: JsonProperty("title")] string Title,
        [property: JsonProperty("description")] string Description
        );
}
