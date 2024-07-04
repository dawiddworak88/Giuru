using Client.Api.Definitions;
using Foundation.GenericRepository.Extensions;
using System;
using System.Linq;

namespace Client.Api.Infrastructure.Fields.Seeds
{
    public static class ClientFieldsSeeds
    {
        public static void SeedClientFields(ClientContext context)
        {
            SeedClientFieldAsync(context, FieldConstants.Zone.Id, FieldConstants.Type.Select, false, FieldConstants.Zone.OptionSetId, "Strefa", "Zone", "Zone");
            SeedOptionsAsync(context, FieldConstants.Zone.ZoneOneId, FieldConstants.Zone.OptionSetId, "Zone 1");
            SeedOptionsAsync(context, FieldConstants.Zone.ZoneTwoId, FieldConstants.Zone.OptionSetId, "Zone 2");

            SeedClientFieldAsync(context, FieldConstants.Transport.Id, FieldConstants.Type.Select, false, FieldConstants.Transport.OptionSetId, "Rodzja transportu", "Type of transport", "Art des Transports");
            SeedOptionsAsync(context, FieldConstants.Transport.OwnPickUpId, FieldConstants.Transport.OptionSetId, "Own PickUp");
            SeedOptionsAsync(context, FieldConstants.Transport.EltapTransportId, FieldConstants.Transport.OptionSetId, "ELTAP Transport");

            SeedClientFieldAsync(context, FieldConstants.Campaign.Id, FieldConstants.Type.Select, false, FieldConstants.Campaign.OptionSetId, "Udział w kampaniach", "Participation in campaigns", "Teilnahme an Kampagnen");
            SeedOptionsAsync(context, FieldConstants.Campaign.TtewId, FieldConstants.Campaign.OptionSetId, "TTEW");
            SeedOptionsAsync(context, FieldConstants.Campaign.OtewId, FieldConstants.Campaign.OptionSetId, "OTEW");
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

        private static void SeedOptionsAsync(
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
