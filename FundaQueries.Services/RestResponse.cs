using System.Net;
using System.Net.Http;

namespace FundaQueries.Services
{
    public class RestResponse<T>
    {
        public T Value { get; }
        public HttpStatusCode StatusCode { get; }
        public string ReasonPhrase { get; }
        public bool IsSuccessStatusCode => ((int)StatusCode >= 200) && ((int)StatusCode <= 299);

        public RestResponse(T value, HttpStatusCode statusCode)
        {
            Value = value;
            StatusCode = statusCode;
        }

        public RestResponse(T value, HttpStatusCode statusCode, string reasonPhrase)
        {
            Value = value;
            StatusCode = statusCode;
            ReasonPhrase = reasonPhrase;
        }

        public static RestResponse<T> Ok(T value, string reasonPhrase = null)
        {
            return new RestResponse<T>(value, HttpStatusCode.OK, reasonPhrase);
        }

        public static RestResponse<T> Unauthorized(string reasonPhrase = null)
        {
            return new RestResponse<T>(default(T), HttpStatusCode.Unauthorized, reasonPhrase);
        }

        public static RestResponse<T> InternalServerError(string reasonPhrase = null)
        {
            return new RestResponse<T>(default(T), HttpStatusCode.InternalServerError, reasonPhrase);
        }
    }
}
