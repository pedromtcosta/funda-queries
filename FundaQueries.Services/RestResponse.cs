using System.Net;
using System.Net.Http;

namespace FundaQueries.Services
{
    public class RestResponse<T>
    {
        public T Value { get; }
        public HttpResponseMessage Response { get; }
        public HttpStatusCode StatusCode => Response.StatusCode;
        public bool IsSuccessStatusCode => Response.IsSuccessStatusCode;

        public RestResponse(T value, HttpResponseMessage response)
        {
            Value = value;
            Response = response;
        }
    }
}
