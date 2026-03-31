using System;
using System.Collections.Generic;

namespace Buyer.Web.Areas.Products.Services.DeliveryMessages
{
    public static class DeliveryTypeResolver
    {
        public static readonly Guid OwnTransport = Guid.Parse("15325239-0ba9-48b6-9d33-08dc65e9a5f8");
        public static readonly Guid StandardDelivery = Guid.Parse("0caf6403-7e80-4b66-9d32-08dc65e9a5f8");

        private static readonly Dictionary<string, string> _map = new()
        {
            { OwnTransport.ToString(), "Own transport" },
            { StandardDelivery.ToString(), "Standard delivery" }
        };

        public static string Resolve(string guid)
        {
            if (!string.IsNullOrWhiteSpace(guid) && _map.TryGetValue(guid, out var name))
                return name;

            return "Unknown";
        }
    }
}
