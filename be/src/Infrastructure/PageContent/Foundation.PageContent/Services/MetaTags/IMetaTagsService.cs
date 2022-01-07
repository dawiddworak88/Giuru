namespace Foundation.PageContent.Services.MetaTags
{
    public interface IMetaTagsService
    {
        string GenerateTitle(string title, bool appendSiteName = true, char separator = '-');
    }
}
