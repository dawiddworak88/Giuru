namespace Foundation.Extensions.Models
{
    public class PagedBaseServiceModel : BaseServiceModel
    {
        public int? PageIndex { get; set; }
        public int? ItemsPerPage { get; set; }
        public string SearchTerm { get; set; }
        public string OrderBy { get; set; }
    }
}
