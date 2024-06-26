using Foundation.GenericRepository.Extensions;
using System;
using System.Linq;

namespace Client.Api.Infrastructure.Fields.Seeds
{
    public static class ClientFieldsSeeds
    {
        public static void SeedClientFields(ClientContext context)
        {
            // zone
            SeedClientFieldAsync(context, Guid.Parse("f9f50ca7-c54f-4f11-4a20-08dc74ab4b1a"), "select", false, Guid.Parse("549d513f-7c0b-4cd6-a412-facec9f4449e"), "Strefa", "Zone", "Zone");
            SeedOptionaAsync(context, Guid.Parse("7123d71e-2b20-418e-f54f-08dc74ab4ff3"), Guid.Parse("549d513f-7c0b-4cd6-a412-facec9f4449e"), "Zone 1");
            SeedOptionaAsync(context, Guid.Parse("c3cfcc71-7218-4949-f550-08dc74ab4ff3"), Guid.Parse("549d513f-7c0b-4cd6-a412-facec9f4449e"), "Zone 2");

            // transport
             SeedClientFieldAsync(context, Guid.Parse("35968b7b-0329-4def-b450-08dc65e9a01d"), "select", false, Guid.Parse("23991c56-c48b-4e82-bb32-58a9588ac63b"), "Rodzja transportu", "Type of transport", "Art des Transports");
            SeedOptionaAsync(context, Guid.Parse("15325239-0ba9-48b6-9d33-08dc65e9a5f8"), Guid.Parse("23991c56-c48b-4e82-bb32-58a9588ac63b"), "Own PickUp");
            SeedOptionaAsync(context, Guid.Parse("0caf6403-7e80-4b66-9d32-08dc65e9a5f8"), Guid.Parse("23991c56-c48b-4e82-bb32-58a9588ac63b"), "ELTAP Transport");

            // campaign
            SeedClientFieldAsync(context, Guid.Parse("89f22cda-ccc1-4102-4a1f-08dc74ab4b1a"), "select", false, Guid.Parse("b6cd0c9b-4214-455f-b5f3-9f7408807088"), "Udział w kampaniach", "Participation in campaigns", "Teilnahme an Kampagnen");
            SeedOptionaAsync(context, Guid.Parse("9b4db26a-7d9a-4ff7-f54d-08dc74ab4ff3"), Guid.Parse("b6cd0c9b-4214-455f-b5f3-9f7408807088"), "TTEW");
            SeedOptionaAsync(context, Guid.Parse("53a23e25-3d43-41f4-f54c-08dc74ab4ff3"), Guid.Parse("b6cd0c9b-4214-455f-b5f3-9f7408807088"), "OTEW");
        }

        private static void SeedClientFieldAsync(
            ClientContext context,
            Guid id,
            string fieldType,
            bool isRequired,
            Guid optionsSetId,
            string filedNamePl,
            string filedNameEn,
            string filedNameDe)
        {
            if (!context.FieldDefinitions.Any(x => x.Id == id))
            {
                var fieldDefinition = new FieldDefinition
                {
                    Id = id,
                    FieldType = fieldType,
                    IsActive = isRequired,
                    OptionSetId = optionsSetId,
                };

                var plFieldDefinitionTranslation = new FieldDefinitionTranslation
                {
                    FieldDefinitionId = id,
                    Language = "pl",
                    FieldName = filedNamePl
                };

                var enFieldDefinitionTranslation = new FieldDefinitionTranslation
                {
                    FieldDefinitionId = id,
                    Language = "en",
                    FieldName = filedNameEn
                };

                var deFieldDefinitionTranslation = new FieldDefinitionTranslation
                {
                    FieldDefinitionId = id,
                    Language = "de",
                    FieldName = filedNameDe
                };

                context.FieldDefinitions.Add(fieldDefinition.FillCommonProperties());

                context.FieldDefinitionTranslations.Add(plFieldDefinitionTranslation.FillCommonProperties());
                context.FieldDefinitionTranslations.Add(enFieldDefinitionTranslation.FillCommonProperties());
                context.FieldDefinitionTranslations.Add(deFieldDefinitionTranslation.FillCommonProperties());

                var optionsSet = new OptionSet
                {
                    Id = optionsSetId
                };

                context.FieldOptionSets.Add(optionsSet.FillCommonProperties());

                context.SaveChanges();
            }
        }

        private static void SeedOptionaAsync(
            ClientContext context,
            Guid id,
            Guid optionsSetId,
            string name)
        {
            if (!context.FieldOptions.Any(x => x.Id == id))
            {
                var fieldOption = new Option
                {
                    Id = id,
                    OptionSetId = optionsSetId
                };

                var fieldOptionsTranslation = new OptionTranslation
                {
                    OptionId = id,
                    Name = name,
                    Language = "en"
                };

                context.FieldOptions.Add(fieldOption.FillCommonProperties());
                context.FieldOptionTranslations.Add(fieldOptionsTranslation.FillCommonProperties());

                context.SaveChanges();
            }
        }
    }
}
