using Buyer.Web.Areas.Home.RepositoryModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Home.Repositories
{
    public interface IHeroSliderRepository
    {
        Task<IEnumerable<HeroSliderItem>> GetHeroSliderItemsAsync(string language, string fallbackLanguage);
    }
}
