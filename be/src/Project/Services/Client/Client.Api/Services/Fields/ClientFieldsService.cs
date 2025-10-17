using Client.Api.Infrastructure;
using Client.Api.Infrastructure.Fields;
using Client.Api.ServicesModels.Fields;
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

namespace Client.Api.Services.Fields
{
    public class ClientFieldsService : IClientFieldsService
    {
        private readonly ClientContext _context;
        private readonly IStringLocalizer<ClientResources> _clientLocalizer;

        public ClientFieldsService(
            IStringLocalizer<ClientResources> clientLocalizer,
            ClientContext context) 
        { 
            _context = context;
            _clientLocalizer = clientLocalizer;
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
                throw new NotFoundException(_clientLocalizer.GetString("FieldDefinitionNotFound"));
            }

            var fieldDefinitionTranslation = fieldDefinition.FieldDefinitionTranslations.FirstOrDefault(x => x.Language == model.Language && x.IsActive);

            if (fieldDefinitionTranslation is null)
            {
                fieldDefinitionTranslation = fieldDefinition.FieldDefinitionTranslations.FirstOrDefault(x => x.IsActive);
            }

            var fieldOptions = _context.FieldOptions.Where(x => x.OptionSetId == fieldDefinition.OptionSetId).ToList();
            var fieldOptionsTranslations = _context.FieldOptionTranslations.Where(x => fieldOptions.Select(y => y.Id).Contains(x.OptionId)).ToList();

            var fieldDefinitionOptions = new List<FieldOptionServiceModel>();

            foreach (var fieldOption in fieldOptions.OrEmptyIfNull()) 
            {
                var newFieldOption = new FieldOptionServiceModel
                {
                    FieldOptionSetId = fieldOption.OptionSetId,
                    Name = fieldOptionsTranslations?.FirstOrDefault(x => x.Language == model.Language && x.OptionId == fieldOption.Id)?.Name ?? fieldOptionsTranslations?.FirstOrDefault(x => x.OptionId == fieldOption.Id)?.Name,
                    Value = fieldOption.Id
                };

                fieldDefinitionOptions.Add(newFieldOption);
            }

            return new ClientFieldServiceModel
            {
                Id = fieldDefinition.Id,
                Name = fieldDefinitionTranslation.FieldName,
                Type = fieldDefinition.FieldType,
                IsRequired = fieldDefinition.IsRequired,
                Options = fieldDefinitionOptions,
                LastModifiedDate = fieldDefinition.LastModifiedDate,
                CreatedDate = fieldDefinition.CreatedDate
            };
        }

        public PagedResults<IEnumerable<ClientFieldServiceModel>> Get(GetClientFieldsServiceModel model)
        {
            var fieldDefinitions = _context.FieldDefinitions.
                Include(fd => fd.FieldDefinitionTranslations)
                .Where(x => x.IsActive)
                .AsSingleQuery();

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                fieldDefinitions = fieldDefinitions.Where(x => x.FieldDefinitionTranslations.Any(y => y.FieldName.StartsWith(model.SearchTerm)));
            }

            fieldDefinitions = fieldDefinitions.ApplySort(model.OrderBy);

            PagedResults<IEnumerable<Infrastructure.Fields.FieldDefinition>> pagedResults;

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                fieldDefinitions = fieldDefinitions.Take(Constants.MaxItemsPerPageLimit);

                pagedResults = fieldDefinitions.PagedIndex(new Pagination(fieldDefinitions.Count(), Constants.MaxItemsPerPageLimit), Constants.DefaultPageIndex);
            }
            else
            {
                pagedResults = fieldDefinitions.PagedIndex(new Pagination(fieldDefinitions.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
            }

            var optionSetIds = pagedResults.Data.Select(x => x.OptionSetId).ToList();

            var fieldOptions = _context.FieldOptions.Where(x => optionSetIds.Contains(x.OptionSetId)).ToList();
            var fieldOptionsTranslations = _context.FieldOptionTranslations.Where(x => fieldOptions.Select(y => y.Id).Contains(x.OptionId)).ToList();

            var fieldDefinitionOptions = new List<FieldOptionServiceModel>();

            foreach (var fieldOption in fieldOptions.OrEmptyIfNull())
            {
                var newFieldOption = new FieldOptionServiceModel
                {
                    FieldOptionSetId = fieldOption.OptionSetId,
                    Name = fieldOptionsTranslations?.FirstOrDefault(x => x.OptionId == fieldOption.Id && x.Language == model.Language)?.Name ?? fieldOptionsTranslations?.FirstOrDefault(x => x.OptionId == fieldOption.Id)?.Name,
                    Value = fieldOption.Id
                };

                fieldDefinitionOptions.Add(newFieldOption);
            }

            return new PagedResults<IEnumerable<ClientFieldServiceModel>>(pagedResults.Total, pagedResults.PageSize)
            {
                Data = pagedResults.Data.OrEmptyIfNull().Select(x => new ClientFieldServiceModel
                {
                    Id = x.Id,
                    Name = x.FieldDefinitionTranslations?.FirstOrDefault(t => t.FieldDefinitionId == x.Id && t.Language == model.Language)?.FieldName ?? x.FieldDefinitionTranslations?.FirstOrDefault(t => t.FieldDefinitionId == x.Id)?.FieldName,
                    Type = x.FieldType,
                    IsRequired = x.IsRequired,
                    Options = fieldDefinitionOptions.Where(y => y.FieldOptionSetId == x.OptionSetId).Select(y => new FieldOptionServiceModel
                    {
                        Name = y.Name,
                        Value = y.Value
                    }),
                    LastModifiedDate = x.LastModifiedDate,
                    CreatedDate = x.CreatedDate
                })
            };
        }

        public async Task DeleteAsync(DeleteClientFieldServiceModel model)
        {
            var fieldDefinition = await _context.FieldDefinitions.FirstOrDefaultAsync(x => x.Id == model.Id);

            if (fieldDefinition is null)
            {
                throw new NotFoundException(_clientLocalizer.GetString("FieldDefinitionNotFound"));
            }

            fieldDefinition.IsActive = false;
            fieldDefinition.LastModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task<Guid> UpdateAsync(UpdateClientFieldServiceModel model)
        {
            var fieldDefinition = await _context.FieldDefinitions
                .Include(x => x.FieldDefinitionTranslations)
                .AsSingleQuery()
                .FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (fieldDefinition is null)
            {
                throw new NotFoundException(_clientLocalizer.GetString("FieldDefinitionNotFound"));
            }

            var fieldDefinitionTranslation = fieldDefinition.FieldDefinitionTranslations.FirstOrDefault(x => x.FieldDefinitionId == fieldDefinition.Id && x.Language == model.Language && x.IsActive);

            if (fieldDefinitionTranslation is not null)
            {
                fieldDefinitionTranslation.FieldName = model.Name;
                fieldDefinitionTranslation.LastModifiedDate = DateTime.UtcNow;
            }
            else
            {
                var newFieldDefinitionTranslation = new FieldDefinitionTranslation
                {
                    FieldDefinitionId = fieldDefinition.Id,
                    FieldName = model.Name,
                    Language = model.Language
                };

                await _context.FieldDefinitionTranslations.AddAsync(newFieldDefinitionTranslation.FillCommonProperties());
            }

            fieldDefinition.IsRequired = model.IsRequired;
            fieldDefinition.FieldType = model.Type;
            fieldDefinition.LastModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return fieldDefinition.Id;
        }
    }
}
