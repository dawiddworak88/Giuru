using Client.Api.Infrastructure;
using Client.Api.Infrastructure.Fields;
using Client.Api.ServicesModels.Fields;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Extensions;
using Microsoft.EntityFrameworkCore;
using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
            var fieldDefinition = await _context.FieldDefinitions
                .Include(fd => fd.FieldDefinitionTranslations)
                .AsSingleQuery()
                .FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (fieldDefinition is null)
            {
                throw new CustomException("", (int)HttpStatusCode.NoContent);
            }

            var fieldDefinitionTranslation = fieldDefinition.FieldDefinitionTranslations.FirstOrDefault(x => x.Language == model.Language && x.IsActive);

            if (fieldDefinitionTranslation is null)
            {
                fieldDefinitionTranslation = fieldDefinition.FieldDefinitionTranslations.FirstOrDefault(x => x.IsActive);
            }

            var fieldDefinitionOptions = from fo in _context.FieldOptions
                                         join fot in _context.FieldOptionsTranslation on fo.Id equals fot.OptionId
                                         join fos in _context.FieldOptionSetTranslations on fo.OptionSetId equals fos.OptionSetId
                                         group new { fo, fot, fos } by fo.OptionSetId into grouped
                                         where grouped.Key == fieldDefinition.OptionSetId
                                         select new ClientFieldOptionServiceModel
                                         {
                                             Name = grouped.FirstOrDefault(g => g.fos.Language == model.Language) != null ? grouped.FirstOrDefault(g => g.fos.Language == model.Language).fos.Name : grouped.FirstOrDefault().fos.Name,
                                             Value = grouped.FirstOrDefault(g => g.fot.Language == model.Language) != null ? grouped.FirstOrDefault(g => g.fot.Language == model.Language).fot.OptionValue : grouped.FirstOrDefault().fot.OptionValue
                                         };


            return new ClientFieldServiceModel
            {
                Id = fieldDefinition.Id,
                Name = fieldDefinitionTranslation.FieldName,
                Type = fieldDefinition.FieldType,
                IsRequired = fieldDefinition.IsRequired,
                Options = fieldDefinitionOptions.OrEmptyIfNull(),
                LastModifiedDate = fieldDefinition.LastModifiedDate,
                CreatedDate = fieldDefinition.CreatedDate
            };
        }
    }
}
