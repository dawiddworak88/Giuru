using Client.Api.Infrastructure;
using Client.Api.Infrastructure.Fields;
using Client.Api.ServicesModels.Fields;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Extensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
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

            if (model.Options.Any())
            {
                var fieldOptionSet = new OptionSet();

                await _context.FieldOptionSets.AddAsync(fieldOptionSet.FillCommonProperties());

                fieldDefinition.OptionSetId = fieldOptionSet.Id;

                foreach (var option in model.Options.OrEmptyIfNull())
                {
                    var fieldOptionSetTranslation = new OptionSetTranslation
                    {
                        OptionSetId = fieldOptionSet.Id,
                        Name = option.Name,
                        Language = model.Language
                    };

                    await _context.FieldOptionSetTranslations.AddAsync(fieldOptionSetTranslation.FillCommonProperties());

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

            await _context.FieldDefinitions.AddAsync(fieldDefinition.FillCommonProperties());

            var fieldDefinitionTranslation = new FieldDefinitionTranslation
            {
                FieldDefinitionId = fieldDefinition.Id,
                FieldName = model.Name,
                Language = model.Language
            };

            await _context.FieldDefinitionTranslations.AddAsync(fieldDefinitionTranslation.FillCommonProperties());

            await _context.SaveChangesAsync();

            return fieldDefinition.Id;
        }

        public async Task<ClientFieldServiceModel> GetAsync(GetClientFieldDefinitionServiceModel model)
        {
            var fieldDefinition = await _context.FieldDefinitions.Include(x => x.FieldDefinitionTranslation).AsSingleQuery().FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (fieldDefinition is null)
            {
                throw new CustomException("", (int)HttpStatusCode.NoContent);
            }

            var fieldDefinitionOptions = from fo in _context.FieldOptions
                                         join fot in _context.FieldOptionsTranslation on fo.Id equals fot.OptionId
                                         select new
                                         {
                                             Value = fot.OptionValue
                                         };

             Console.WriteLine(JsonConvert.SerializeObject(fieldDefinitionOptions));

            return new ClientFieldServiceModel
            {
                Id = model.Id,
                Name = fieldDefinition.FieldDefinitionTranslation.FieldName,
                Type = fieldDefinition.FieldType,
                LastModifiedDate = fieldDefinition.LastModifiedDate,
                CreatedDate = fieldDefinition.CreatedDate
            };
        }
    }
}
