using Foundation.ApiExtensions.Communications;
using System.Threading.Tasks;

namespace Foundation.ApiExtensions.Services
{
    public interface IApiClientService
    {
        Task<ApiResponse<T>> PostAsync<S, W, T>(S request) where S : ApiRequest<W>;
    }
}
