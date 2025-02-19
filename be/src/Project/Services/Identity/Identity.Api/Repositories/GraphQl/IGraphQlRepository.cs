using System.Threading.Tasks;

namespace Identity.Api.Repositories.GraphQl
{
    public interface IGraphQlRepository
    {
        Task<string> GetTextAsync(string language, string fallbackLanguage, string attribute);
    }
}
