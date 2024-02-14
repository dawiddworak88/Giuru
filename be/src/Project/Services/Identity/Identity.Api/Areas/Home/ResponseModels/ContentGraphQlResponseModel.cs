using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Collections.Generic;

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