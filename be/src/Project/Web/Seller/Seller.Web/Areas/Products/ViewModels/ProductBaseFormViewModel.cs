using Foundation.PageContent.Components.ListItems.ViewModels;
using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Products.ViewModels
{
    public class ProductBaseFormViewModel
    {
        public string Title { get; set; }
        public string SelectCategoryLabel { get; set; }
        public string SelectPrimaryProductLabel { get; set; }
        public string ProductFilesLabel { get; set; }
        public string ProductPicturesLabel { get; set; }
        public string SkuLabel { get; set; }
        public string NameLabel { get; set; }
        public string DescriptionLabel { get; set; }
        public string IsNewLabel { get; set; }
        public string IsPublishedLabel { get; set; }
        public string SaveText { get; set; }
        public string ProductsSuggestionUrl { get; set; }
        public string NoGroupsText { get; set; }
        public string GroupsLabel { get; set; }
        public string ProductsUrl { get; set; }
        public string IdLabel { get; set; }
        public string EanLabel { get; set; }
        public string DropOrSelectFilesLabel { get; set; }
        public string DropFilesLabel { get; set; }
        public bool IsUploadInChunksEnabled { get; set; }
        public int? ChunkSize { get; set; }
        public string SaveMediaUrl { get; set; }
        public string SaveMediaChunkUrl { get; set; }
        public string SaveMediaChunkCompleteUrl { get; set; }
        public string DeleteLabel { get; set; }
        public string NameRequiredErrorMessage { get; set; }
        public string EnterNameText { get; set; }
        public string EnterSkuText { get; set; }
        public string SkuRequiredErrorMessage { get; set; }
        public string SaveUrl { get; set; }
        public string GeneralErrorMessage { get; set; }
        public string GetCategorySchemaUrl { get; set; }
        public string NavigateToProductsLabel { get; set; }
        public IEnumerable<ListItemViewModel> Categories { get; set; }
        public IEnumerable<ListItemViewModel> PrimaryProducts { get; set; }
        public IEnumerable<ListItemViewModel> Groups { get; set; }
    }
}
