﻿using Foundation.PageContent.Components.ListItems.ViewModels;
using Seller.Web.Shared.ViewModels;
using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Products.ViewModels
{
    public class ProductFormViewModel
    {
        public string Title { get; set; }
        public Guid? Id { get; set; }
        public Guid? CategoryId { get; set; }
        public string Sku { get; set; }
        public int? FulfillmentTime { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<FileViewModel> Images { get; set; }
        public IEnumerable<FileViewModel> Files  { get; set; }
        public string Schema { get; set; }
        public string UiSchema { get; set; }
        public string FormData { get; set; }
        public bool IsNew { get; set; }
        public bool IsPublished { get; set; }
        public string SelectCategoryLabel { get; set; }
        public string SelectPrimaryProductLabel { get; set; }
        public string ProductFilesLabel { get; set; }
        public string ProductPicturesLabel { get; set; }
        public string SkuLabel { get; set; }
        public string FulfillmentTimeLabel { get; set; }
        public string NameLabel { get; set; }
        public string DescriptionLabel { get; set; }
        public string IsNewLabel { get; set; }
        public string IsPublishedLabel { get; set; }
        public string SaveText { get; set; }
        public IEnumerable<ListItemViewModel> Categories  { get; set; }
        public IEnumerable<ListItemViewModel> PrimaryProducts { get; set; }
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
        public string ProductsUrl { get; set; }
        public string IdLabel { get; set; }
        public string EanLabel { get; set; }
        public string Ean { get; set; }
        public ListItemViewModel PrimaryProduct { get; set; }
        public string ProductsSuggestionUrl { get; set; }
        public long VideoFileSizeLimit { get; set; }
        public string FileSizeLimitErrorMessage { get; set; }
    }
}