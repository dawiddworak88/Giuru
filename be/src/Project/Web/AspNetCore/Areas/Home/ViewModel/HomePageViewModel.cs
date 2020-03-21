using AspNetCore.Shared.Footers.ViewModels;
using AspNetCore.Shared.Headers.ViewModels;

namespace AspNetCore.Areas.Home.ViewModel
{
    public class HomePageViewModel
    {
        public HeaderViewModel Header { get; set; }
        public FooterViewModel Footer { get; set; }
        public string Welcome { get; set; }
        public string LearnMore { get; set; }
    }
}
