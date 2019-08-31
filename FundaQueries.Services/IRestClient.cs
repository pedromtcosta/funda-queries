using System.Threading.Tasks;

namespace FundaQueries.Services
{
    public interface IRestClient
    {
        Task<RestResponse<T>> GetAsync<T>(string requestUri);
    }
}
