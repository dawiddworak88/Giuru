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
                throw new CustomException(_clientLocalizer.GetString("FieldDefinitionNotFound"), (int)HttpStatusCode.NoContent);
            }
            else if (fieldDefinition.FieldType != FieldTypesConstants.SelectFieldType)
            {
                throw new CustomException(_clientLocalizer.GetString("FieldDefinitionSelectTypeConflict"), (int)HttpStatusCode.Conflict);
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

        public async Task DeleteAsync(DeleteClientFieldOptionServiceModel model)
        {
            var fieldOption = await _context.FieldOptions.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (fieldOption is null)
            {
                throw new CustomException(_clientLocalizer.GetString("FieldOptionNotFound"), (int)HttpStatusCode.NoContent);
            }

            fieldOption.IsActive = false;
            fieldOption.LastModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public PagedResults<IEnumerable<FieldOptionServiceModel>> Get(GetClientFieldOptionsServiceModel model)
        {
            var fieldOption = _context.FieldOptions
                .Include(x => x.OptionsTranslations)
                .Include(x => x.OptionSet)
                .Include(x => x.OptionSet.OptionSetTranslations)
                .AsSingleQuery();

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                fieldOption = fieldOption.Where(x => x.OptionsTranslations.Any(y => y.OptionValue.StartsWith(model.SearchTerm)));
            }

            fieldOption = fieldOption.ApplySort(model.OrderBy);

            PagedResults<IEnumerable<Option>> pagedResults;

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                fieldOption = fieldOption.Take(Constants.MaxItemsPerPageLimit);

                pagedResults = fieldOption.PagedIndex(new Pagination(fieldOption.Count(), Constants.MaxItemsPerPageLimit), Constants.DefaultPageIndex);
            }
            else
            {
                pagedResults = fieldOption.PagedIndex(new Pagination(fieldOption.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
            }

            return new PagedResults<IEnumerable<FieldOptionServiceModel>>(pagedResults.Total, pagedResults.PageSize)
            {
                Data = pagedResults.Data.OrEmptyIfNull().Select(x => new FieldOptionServiceModel
                {
                    Id = x.Id,
                    Name = x.OptionSet?.OptionSetTranslations?.FirstOrDefault(t => t.Language == model.Language && t.IsActive)?.Name ?? x.OptionSet?.OptionSetTranslations?.FirstOrDefault(t => t.IsActive)?.Name,
                    Value = x.OptionsTranslations?.FirstOrDefault(t => t.Language == model.Language && t.IsActive)?.OptionValue ?? x.OptionsTranslations?.FirstOrDefault(t => t.IsActive)?.OptionValue,
                    LastModifiedDate = x.LastModifiedDate,
                    CreatedDate = x.CreatedDate
                })
            };
        }

        public async Task<FieldOptionServiceModel> GetAsync(GetClientFieldOptionServiceModel model)
        {
            var fieldOption = await _context.FieldOptions
                .Include(x => x.OptionsTranslations)
                .Include(x => x.OptionSet)
                .Include(x => x.OptionSet.OptionSetTranslations)
                .AsSingleQuery()
                .FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (fieldOption is null)
            {
                throw new CustomException(_clientLocalizer.GetString("FieldOptionNotFound"), (int)HttpStatusCode.NoContent);
            }

            return new FieldOptionServiceModel
            {
                Id = model.Id,
                Name = fieldOption.OptionSet?.OptionSetTranslations?.FirstOrDefault(x => x.Language == model.Language && x.IsActive)?.Name ?? fieldOption.OptionSet?.OptionSetTranslations?.FirstOrDefault(x => x.IsActive)?.Name,
                Value = fieldOption.OptionsTranslations?.FirstOrDefault(x => x.Language == model.Language && x.IsActive)?.OptionValue ?? fieldOption.OptionsTranslations?.FirstOrDefault(x => x.IsActive)?.OptionValue,
                LastModifiedDate = fieldOption.LastModifiedDate,
                CreatedDate = fieldOption.CreatedDate
            };
        }

        public async Task<Guid> UpdateAsync(UpdateClientFieldOptionServiceModel model)
        {
            var fieldOption = await _context.FieldOptions
                .Include(x => x.OptionsTranslations)
                .Include(x => x.OptionSet)
                .Include(x => x.OptionSet.OptionSetTranslations)
                .AsSingleQuery()
                .FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (fieldOption is null)
            {
                throw new CustomException(_clientLocalizer.GetString("FieldOptionNotFound"), (int)HttpStatusCode.NoContent);
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
