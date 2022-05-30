using System;

namespace Seller.Web.Shared.Definitions
{
    public static class Constants
    {
        public static Guid ProductEntityTypeId { get; } = Guid.Parse("AB5B9E94-0735-432E-1C4A-08D7F9CE3C60");
        public static readonly int PreviewMaxWidth = 200;
        public static readonly int PreviewMaxHeight = 200;
        public static readonly int ProductsSuggestionDefaultPageIndex = 1;
        public static readonly int ProductsSuggestionDefaultItemsPerPage = 20;
    }
}
