﻿using Buyer.Web.Areas.DownloadCenter.DomainModels;
using Buyer.Web.Areas.DownloadCenter.Repositories;
using Buyer.Web.Areas.DownloadCenter.ViewModel;
using Buyer.Web.Shared.ComponentModels.Files;
using Buyer.Web.Shared.Definitions.Files;
using Buyer.Web.Shared.ViewModels.Files;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.DownloadCenter.ModelBuilders
{
    public class DownloadCenterCategoryDetailsModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, DownloadCenterCategoryDetailsViewModel>
    {
        private readonly IAsyncComponentModelBuilder<FilesComponentModel, DownloadCenterFilesViewModel> filesModelBuilder;
        private readonly IDownloadCenterRepository downloadCenterRepository;
        private readonly LinkGenerator linkGenerator;

        public DownloadCenterCategoryDetailsModelBuilder(
            IAsyncComponentModelBuilder<FilesComponentModel, DownloadCenterFilesViewModel> filesModelBuilder,
            IDownloadCenterRepository downloadCenterRepository,
            LinkGenerator linkGenerator)
        {
            this.filesModelBuilder = filesModelBuilder;
            this.downloadCenterRepository = downloadCenterRepository;
            this.linkGenerator = linkGenerator;
        }

        public async Task<DownloadCenterCategoryDetailsViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new DownloadCenterCategoryDetailsViewModel();

            if (componentModel.Id.HasValue)
            {
                var downloadCenterCategory = await this.downloadCenterRepository.GetAsync(componentModel.Token, componentModel.Language, componentModel.Id);

                if (downloadCenterCategory is not null)
                {
                    viewModel.Title = downloadCenterCategory.CategoryName;
                    viewModel.Subcategories = downloadCenterCategory.Subcategories.OrEmptyIfNull().Select(x => new DownloadCenterCategoryDetailViewModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Url = x.Url
                    });
                }

                var downloadCenterFiles = await this.downloadCenterRepository.GetCategoryFilesAsync(componentModel.Token, componentModel.Language, componentModel.Id, FilesConstants.DefaultPageIndex, FilesConstants.DefaultPageSize, null, $"{nameof(DownloadCenterFile.CreatedDate)} desc");

                if (downloadCenterFiles is not null)
                {
                    var filesComponentModel = new FilesComponentModel
                    {
                        Id = componentModel.Id,
                        Token = componentModel.Token,
                        Language = componentModel.Language,
                        Files = downloadCenterFiles.Data.OrEmptyIfNull().Select(x => x.Id),
                        SearchApiUrl = this.linkGenerator.GetPathByAction("Get", "DownloadCenterApi", new { Area = "DownloadCenter", culture = CultureInfo.CurrentUICulture.Name })
                    };

                    viewModel.Files = await this.filesModelBuilder.BuildModelAsync(filesComponentModel);
                }
            }

            return viewModel;
        }
    }
}
