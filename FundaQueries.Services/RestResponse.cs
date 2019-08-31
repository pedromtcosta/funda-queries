using System.Net;
using System.Net.Http;

namespace FundaQueries.Services
{
    public class RestResponse<T>
    {
        public T Value { get; }
        public HttpStatusCode StatusCode { get; }

        public RestResponse(T value, HttpStatusCode statusCode)
        {
            Value = value;
            StatusCode = statusCode;
        }

        public static RestResponse<T> Ok(T value)
        {
            return new RestResponse<T>(value, HttpStatusCode.OK);
        }
    }
}
