using Client.Api.Infrastructure;
using Client.Api.Infrastructure.Fields;
using Client.Api.ServicesModels.FieldOptions;
using Foundation.Extensions.Exceptions;
using Foundation.GenericRepository.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Client.Api.Services.FieldOptions
{
    public class ClientFieldOptionsService : IClientFieldOptionsService
    {
        private readonly ClientContext _context;

        public ClientFieldOptionsService(
            ClientContext context)
        {
            _context = context;
        }
        public async Task<Guid> CreateAsync(CreateFieldOptionServiceModel model)
        {
            var fieldDefinition = await _context.FieldDefinitions.FirstOrDefaultAsync(x => x.Id == model.FieldDefinitionId && x.IsActive);

            if (fieldDefinition is null)
            {
                throw new CustomException("", (int)HttpStatusCode.NoContent);
            }
            else if (fieldDefinition.FieldType != "select")
            {
                throw new CustomException("Is not select", (int)HttpStatusCode.Conflict);
            }

            var fieldOptionSetId = fieldDefinition.OptionSetId;

            if (fieldDefinition.OptionSetId.HasValue is false)
            {
                var fieldOptionSet = new OptionSet();

                await _context.FieldOptionSets.AddAsync(fieldOptionSet.FillCommonProperties());

                fieldOptionSetId = fieldOptionSet.Id;
                fieldDefinition.OptionSetId = fieldOptionSetId.Value;
            }

            var fieldOptionSetTranslation = new OptionSetTranslation
            {
                OptionSetId = fieldOptionSetId.Value,
                Name = model.Name,
                Language = model.Language
            };
            
            await _context.FieldOptionSetTranslations.AddAsync(fieldOptionSetTranslation.FillCommonProperties());
           
            var fieldOption = new Option
            {
                OptionSetId = fieldOptionSetId.Value
            };
           
            await _context.FieldOptions.AddAsync(fieldOption.FillCommonProperties());
            
            var fieldOptionTranslation = new OptionTranslation
            {
                OptionId = fieldOption.Id,
                OptionValue = model.Value,
                Language = model.Language
            };

            await _context.FieldOptionsTranslation.AddAsync(fieldOptionTranslation.FillCommonProperties());

            await _context.SaveChangesAsync();

            return fieldOption.Id;
        }

        public async Task<Guid> UpdateAsync(UpdateFieldOptionServiceModel model)
        {
            var fieldOption = await _context.FieldOptions
                .Include(x => x.OptionsTranslations)
                .Include(x => x.OptionSet)
                .Include(x => x.OptionSet.OptionSetTranslations)
                .AsSingleQuery()
                .FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (fieldOption is null)
            {
                throw new CustomException("", (int)HttpStatusCode.NoContent);
            }

            var fieldOptionSetTranslation = fieldOption.OptionSet.OptionSetTranslations.FirstOrDefault(x => x.Language == model.Language && x.IsActive);

            if (fieldOptionSetTranslation is not null)
            {
                fieldOptionSetTranslation.Name = model.Name;
            }
            else
            {
                var newFieldOptionSetTranslation = new OptionSetTranslation
                {
                    OptionSetId = fieldOption.OptionSetId,
                    Name = model.Name,
                    Language = model.Language
                };

                await _context.FieldOptionSetTranslations.AddAsync(newFieldOptionSetTranslation.FillCommonProperties());
            }

            var fieldOptionTranslation = fieldOption.OptionsTranslations.FirstOrDefault(x => x.Language == model.Language && x.IsActive);

            if (fieldOptionTranslation is not null)
            {
                fieldOptionTranslation.OptionValue = model.Value;                
            }
            else
            {
                var newFieldOptionTranslation = new OptionTranslation
                {
                    OptionId = fieldOption.Id,
                    OptionValue = model.Value,
                    Language = model.Language
                };

                await _context.FieldOptionsTranslation.AddAsync(newFieldOptionTranslation.FillCommonProperties());
            }

            fieldOption.LastModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return fieldOption.Id;
        }
    }
}
