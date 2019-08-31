using System.Net;
using System.Net.Http;

namespace FundaQueries.Services
{
    public class RestResponse<T>
    {
        public T Value { get; }
        public HttpStatusCode StatusCode { get; }
        public bool IsSuccessStatusCode => ((int)StatusCode >= 200) && ((int)StatusCode <= 299);

        public RestResponse(T value, HttpStatusCode statusCode)
        {
            Value = value;
            StatusCode = statusCode;
        }

        public static RestResponse<T> Ok(T value)
        {
            return new RestResponse<T>(value, HttpStatusCode.OK);
        }

        public static RestResponse<T> InternalServerError()
        {
            return new RestResponse<T>(default(T), HttpStatusCode.InternalServerError);
        }
    }
}
