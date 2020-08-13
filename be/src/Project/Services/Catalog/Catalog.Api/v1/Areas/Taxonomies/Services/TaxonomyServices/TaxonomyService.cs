using Foundation.Extensions.Definitions;
using Foundation.GenericRepository.Services;
using Catalog.Api.v1.Areas.Taxonomies.Models;
using Catalog.Api.v1.Areas.Taxonomies.ResultModels;
using Catalog.Api.v1.Areas.Taxonomies.Validators;
using System;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Api.Infrastructure.Taxonomies.Entities;
using Catalog.Api.Infrastructure;

namespace Catalog.Api.v1.Areas.Taxonomies.Services.TaxonomyServices
{
    public class TaxonomyService : ITaxonomyService
    {
        private readonly CatalogContext context;
        private readonly IEntityService entityService;

        public TaxonomyService(
            CatalogContext context,
            IEntityService entityService
            )
        {
            this.context = context;
            this.entityService = entityService;
        }

        public async Task<TaxonomyResultModel> CreateAsync(CreateTaxonomyModel model)
        {
            //var validator = new CreateTaxonomyModelValidator();

            //var validationResult = await validator.ValidateAsync(model);

            //var createTaxonomyResultModel = new TaxonomyResultModel();

            //if (!validationResult.IsValid)
            //{
            //    createTaxonomyResultModel.Errors.AddRange(validationResult.Errors.Select(x => x.ErrorMessage));
            //    return createTaxonomyResultModel;
            //}

            //var seller = this.sellerRepository.GetById(model.SellerId.Value);

            //if (seller == null)
            //{
            //    createTaxonomyResultModel.Errors.Add(ErrorConstants.NoSeller);
            //    return createTaxonomyResultModel;
            //}

            //var taxonomy = new Taxonomy
            //{
            //    Name = model.Name,
            //    ParentId = model.ParentId
            //};

            //await this.context.Taxonomies.AddAsync(this.entityService.EnrichEntity(taxonomy, model.Username));

            //await this.context.SaveChangesAsync();

            //var translation = new Translation
            //{
            //    Key = taxonomy.Id.ToString(),
            //    Value = taxonomy.Name,
            //    Language = model.Language
            //};

            //await this.context.Translations.AddAsync(this.entityService.EnrichEntity(translation, model.Username));

            //await this.context.SaveChangesAsync();

            //createTaxonomyResultModel.Taxonomy = taxonomy;

            //return createTaxonomyResultModel;

            return default;
        }

        public async Task<TaxonomyResultModel> GetByName(GetTaxonomyModel model)
        {
            //var validator = new GetTaxonomyModelValidator();

            //var validationResult = await validator.ValidateAsync(model);

            //var getTaxonomyResultModel = new TaxonomyResultModel();

            //if (!validationResult.IsValid)
            //{
            //    getTaxonomyResultModel.Errors.AddRange(validationResult.Errors.Select(x => x.ErrorMessage));
            //    return getTaxonomyResultModel;
            //}

            //var seller = this.sellerRepository.GetById(model.SellerId.Value);

            //if (seller == null)
            //{
            //    getTaxonomyResultModel.Errors.Add(ErrorConstants.NoSeller);
            //    return getTaxonomyResultModel;
            //}

            //if (model.RootId.HasValue)
            //{
            //    var taxonomies = this.context.Taxonomies.Where(x => x.Name == model.Name && x.IsActive);

            //    foreach (var taxonomy in taxonomies)
            //    {
            //        Guid? rootId = taxonomy.ParentId;
            //        var rootTaxonomy = taxonomy;

            //        while (rootId.HasValue)
            //        {
            //            rootTaxonomy = this.context.Taxonomies.FirstOrDefault(x => x.Id == rootId.Value && x.IsActive);
            //            rootId = rootTaxonomy.ParentId;
            //        }

            //        if (model.RootId == rootTaxonomy.Id)
            //        {
            //            return new TaxonomyResultModel
            //            {
            //                Taxonomy = taxonomy
            //            };
            //        }
            //    }
            //}

            //return new TaxonomyResultModel
            //{
            //    Taxonomy = this.context.Taxonomies.FirstOrDefault(x => x.Name == model.Name && x.ParentId == null && x.IsActive)
            //};

            return default;
        }
    }
}

