using Client.Api.Infrastructure;
using Client.Api.Infrastructure.Fields;
using Client.Api.ServicesModels.Fields;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Api.Services.Fields
{
    public class ClientFieldsService : IClientFieldsService
    {
        private readonly ClientContext _context;

        public ClientFieldsService(
            ClientContext context) 
        { 
            _context = context;
        }

        public async Task<Guid> CreateAsync(CreateClientFieldServiceModel model)
        {
            var fieldDefinition = new Infrastructure.Fields.FieldDefinition
            {
                FieldType = model.Type,
                IsRequired = model.IsRequired
            };

            await _context.FieldDefinitions.AddAsync(fieldDefinition.FillCommonProperties());

            var fieldDefinitionTranslation = new FieldDefinitionTranslation
            {
                FieldDefinitionId = fieldDefinition.Id,
                FieldName = model.Name,
                Language = model.Language
            };

            await _context.FieldDefinitionTranslations.AddAsync(fieldDefinitionTranslation.FillCommonProperties());

            if (model.Options.Any())
            {
                var fieldOptionSet = new OptionSet();

                await _context.FieldOptionSets.AddAsync(fieldOptionSet.FillCommonProperties());

                var fieldOptionSetTranslation = new OptionSetTranslation
                {
                    OptionSetId = fieldOptionSet.Id,
                    Name = model.Name,
                    Language = model.Language
                };

                await _context.FieldOptionSetTranslations.AddAsync(fieldOptionSetTranslation.FillCommonProperties());

                foreach (var option in  model.Options.OrEmptyIfNull())
                {
                    var fieldOption = new Option
                    {
                        OptionSetId = fieldOptionSet.Id
                    };

                    await _context.FieldOptions.AddAsync(fieldOption.FillCommonProperties());

                    var fieldOptionTranslation = new OptionTranslation
                    {
                        OptionId = fieldOption.Id,
                        OptionValue = option.Value,
                        Language = model.Language
                    };

                    await _context.FieldOptionsTranslation.AddAsync(fieldOptionTranslation.FillCommonProperties());
                }
            }

            await _context.SaveChangesAsync();

            return fieldDefinition.Id;
        }
    }
}
