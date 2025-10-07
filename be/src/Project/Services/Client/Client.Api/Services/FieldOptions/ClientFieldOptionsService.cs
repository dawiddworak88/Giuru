using Client.Api.Definitions;
using Client.Api.Infrastructure;
using Client.Api.Infrastructure.Fields;
using Client.Api.ServicesModels.FieldOptions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Definitions;
using Foundation.GenericRepository.Extensions;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Client.Api.Services.FieldOptions
{
    public class ClientFieldOptionsService : IClientFieldOptionsService
    {
        private readonly ClientContext _context;
        private readonly IStringLocalizer<ClientResources> _clientLocalizer;

        public ClientFieldOptionsService(
            IStringLocalizer<ClientResources> clientLocalizer,
            ClientContext context)
        {
            _context = context;
            _clientLocalizer = clientLocalizer;
        }
        public async Task<Guid> CreateAsync(CreateClientFieldOptionServiceModel model)
        {
            var fieldDefinition = await _context.FieldDefinitions.FirstOrDefaultAsync(x => x.Id == model.FieldDefinitionId && x.IsActive);

            if (fieldDefinition is null)
            {
                throw new NotFoundException(_clientLocalizer.GetString("FieldDefinitionNotFound"));
            }
            else if (fieldDefinition.FieldType != FieldTypesConstants.SelectFieldType)
            {
                throw new ConflictException(_clientLocalizer.GetString("FieldDefinitionSelectTypeConflict"));
            }

            var fieldOptionSetId = fieldDefinition.OptionSetId;

            if (fieldDefinition.OptionSetId.HasValue is false)
            {
                var fieldOptionSet = new OptionSet();

                await _context.FieldOptionSets.AddAsync(fieldOptionSet.FillCommonProperties());

                fieldOptionSetId = fieldOptionSet.Id;
                fieldDefinition.OptionSetId = fieldOptionSetId.Value;
            }

            var fieldOption = new Option
            {
                OptionSetId = fieldOptionSetId.Value
            };
           
            await _context.FieldOptions.AddAsync(fieldOption.FillCommonProperties());
            
            var fieldOptionTranslation = new OptionTranslation
            {
                OptionId = fieldOption.Id,
                Name = model.Name,
                Language = model.Language
            };

            await _context.FieldOptionTranslations.AddAsync(fieldOptionTranslation.FillCommonProperties());

            await _context.SaveChangesAsync();

            return fieldOption.Id;
        }

        public async Task DeleteAsync(DeleteClientFieldOptionServiceModel model)
        {
            var fieldOption = await _context.FieldOptions.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (fieldOption is null)
            {
                throw new NotFoundException(_clientLocalizer.GetString("FieldOptionNotFound"));
            }

            fieldOption.IsActive = false;
            fieldOption.LastModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public PagedResults<IEnumerable<ClientFieldOptionServiceModel>> Get(GetClientFieldOptionsServiceModel model)
        {
            var fieldOptions = _context.FieldOptions
                .Include(x => x.OptionsTranslations)
                .Where(x => x.IsActive)
                .AsSingleQuery();

            if (model.FieldDefinitionId.HasValue)
            {
                var fieldDefinition = _context.FieldDefinitions.FirstOrDefault(x => x.Id == model.FieldDefinitionId && x.IsActive);

                if (fieldDefinition is not null)
                {
                    fieldOptions = fieldOptions.Where(x => x.OptionSetId == fieldDefinition.OptionSetId);
                }
            }

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                fieldOptions = fieldOptions.Where(x => x.OptionsTranslations.Any(y => y.Name.StartsWith(model.SearchTerm)));
            }

            fieldOptions = fieldOptions.ApplySort(model.OrderBy);

            PagedResults<IEnumerable<Option>> pagedResults;

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                fieldOptions = fieldOptions.Take(Constants.MaxItemsPerPageLimit);

                pagedResults = fieldOptions.PagedIndex(new Pagination(fieldOptions.Count(), Constants.MaxItemsPerPageLimit), Constants.DefaultPageIndex);
            }
            else
            {
                pagedResults = fieldOptions.PagedIndex(new Pagination(fieldOptions.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
            }

            var optionsTranslations = _context.FieldOptionTranslations.Where(x => pagedResults.Data.Select(y => y.Id).Contains(x.OptionId)).ToList();

            return new PagedResults<IEnumerable<ClientFieldOptionServiceModel>>(pagedResults.Total, pagedResults.PageSize)
            {
                Data = pagedResults.Data.OrEmptyIfNull().Select(x => new ClientFieldOptionServiceModel
                {
                    Id = x.Id,
                    Name = optionsTranslations?.FirstOrDefault(t => t.Language == model.Language && t.OptionId == x.Id)?.Name ?? optionsTranslations?.FirstOrDefault(t => t.OptionId == x.Id)?.Name,
                    Value = x.Id,
                    LastModifiedDate = x.LastModifiedDate,
                    CreatedDate = x.CreatedDate
                })
            };
        }

        public async Task<ClientFieldOptionServiceModel> GetAsync(GetClientFieldOptionServiceModel model)
        {
            var fieldOption = await _context.FieldOptions
                .Include(x => x.OptionsTranslations)
                .AsSingleQuery()
                .FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (fieldOption is null)
            {
                throw new NotFoundException(_clientLocalizer.GetString("FieldOptionNotFound"));
            }

            var fieldDefinition = await _context.FieldDefinitions.FirstOrDefaultAsync(x => x.OptionSetId == fieldOption.OptionSetId);

            return new ClientFieldOptionServiceModel
            {
                Id = model.Id,
                Name = fieldOption.OptionsTranslations?.FirstOrDefault(x => x.Language == model.Language && x.IsActive)?.Name ?? fieldOption.OptionsTranslations?.FirstOrDefault(x => x.IsActive)?.Name,
                Value = fieldOption?.Id,
                FieldDefinitionId = fieldDefinition?.Id,
                LastModifiedDate = fieldOption.LastModifiedDate,
                CreatedDate = fieldOption.CreatedDate
            };
        }

        public async Task<Guid> UpdateAsync(UpdateClientFieldOptionServiceModel model)
        {
            var fieldOption = await _context.FieldOptions
                .Include(x => x.OptionsTranslations)
                .AsSingleQuery()
                .FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (fieldOption is null)
            {
                throw new NotFoundException(_clientLocalizer.GetString("FieldOptionNotFound"));
            }

            var fieldOptionTranslation = fieldOption.OptionsTranslations.FirstOrDefault(x => x.Language == model.Language && x.IsActive);

            if (fieldOptionTranslation is not null)
            {
                fieldOptionTranslation.Name = model.Name;                
            }
            else
            {
                var newFieldOptionTranslation = new OptionTranslation
                {
                    OptionId = fieldOption.Id,
                    Name = model.Name,
                    Language = model.Language
                };

                await _context.FieldOptionTranslations.AddAsync(newFieldOptionTranslation.FillCommonProperties());
            }

            fieldOption.LastModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return fieldOption.Id;
        }
    }
}
