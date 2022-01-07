using System.Threading.Tasks;

namespace Foundation.Extensions.ModelBuilders
{
    public interface IAsyncComponentModelBuilder<in T, S> where T : class where S : class
    {
        Task<S> BuildModelAsync(T componentModel);
    }
}
