using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace FundaQueries.Services
{
    public class RestClient : IRestClient
    {
        public async Task<RestResponse<T>> GetAsync<T>(string requestUri)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(requestUri);
                var json = await response.Content.ReadAsStringAsync();
                var value = JsonConvert.DeserializeObject<T>(json);

                return new RestResponse<T>(value, response.StatusCode);
            }
        }
    }
}
