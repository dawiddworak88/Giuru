namespace Foundation.PageContent.Services.MetaTags
{
    public class MetaTagsService : IMetaTagsService
    {
        public string GenerateTitle(string title, bool appendSiteName = true, char separator = '-')
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return Definitions.MetaTagsConstants.SiteName;
            }
            else
            {
                if (appendSiteName)
                {
                    return $"{title} {separator} {Definitions.MetaTagsConstants.SiteName}";
                }
                else
                {
                    return title;
                }
            }
        }
    }
}
