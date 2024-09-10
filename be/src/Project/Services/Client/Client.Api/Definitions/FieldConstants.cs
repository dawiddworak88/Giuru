using System;

namespace Client.Api.Definitions
{
    public static class FieldConstants
    {
        public struct Type
        {
            public static readonly string Select = "select";
        }

        public struct Transport
        {
            public static readonly Guid Id = Guid.Parse("35968b7b-0329-4def-b450-08dc65e9a01d");
            public static readonly Guid CompanyTransportId = Guid.Parse("0caf6403-7e80-4b66-9d32-08dc65e9a5f8");
            public static readonly Guid OwnPickUpId = Guid.Parse("15325239-0ba9-48b6-9d33-08dc65e9a5f8");
            public static readonly Guid OptionSetId = Guid.Parse("549d513f-7c0b-4cd6-a412-facec9f4449e");
        }

        public struct Campaign
        {
            public static readonly Guid Id = Guid.Parse("89f22cda-ccc1-4102-4a1f-08dc74ab4b1a");
            public static readonly Guid OtewId = Guid.Parse("53a23e25-3d43-41f4-f54c-08dc74ab4ff3");
            public static readonly Guid TtewId = Guid.Parse("9b4db26a-7d9a-4ff7-f54d-08dc74ab4ff3");
            public static readonly Guid OptionSetId = Guid.Parse("23991c56-c48b-4e82-bb32-58a9588ac63b");
        }

        public struct Zone
        {
            public static readonly Guid Id = Guid.Parse("f9f50ca7-c54f-4f11-4a20-08dc74ab4b1a");
            public static readonly Guid ZoneOneId = Guid.Parse("7123d71e-2b20-418e-f54f-08dc74ab4ff3");
            public static readonly Guid ZoneTwoId = Guid.Parse("c3cfcc71-7218-4949-f550-08dc74ab4ff3");
            public static readonly Guid OptionSetId = Guid.Parse("b6cd0c9b-4214-455f-b5f3-9f7408807088");
        }
    }
}
