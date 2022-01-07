using System;

namespace Foundation.Extensions.Helpers
{
    public static class GuidHelper
    {
        public static Guid? ParseNullable(string guid)
        {
            return !string.IsNullOrWhiteSpace(guid) ? Guid.Parse(guid) : (Guid?)null;
        }
    }
}
