using Data.WebClient.Enums;
using Data.WebClient.Exceptions;
using Kernel.Constants;
using Kernel.Exceptions;
using Newtonsoft.Json;
using System.Net;

namespace Data.WebClient.Helpers
{
    public static class ErrorHandlerHelper
    {
        public static object HandleError(HttpResponseMessage responseMessage, string responseData, ErrorHandlingMode handlingMode)
        {
            return handlingMode switch
            {
                ErrorHandlingMode.Ignore => null,
                ErrorHandlingMode.Manual => throw new WebClientException(responseMessage.StatusCode, string.IsNullOrEmpty(responseData) ? string.Empty : responseData),
                ErrorHandlingMode.Auto => GetAutoException(responseMessage.StatusCode, responseData),
                ErrorHandlingMode.Debug => throw new WebClientException(responseMessage.StatusCode, $"{JsonConvert.SerializeObject(responseMessage)}"),

                _ => throw new WebClientException(responseMessage.StatusCode, responseData),
            };
        }

        private static object GetAutoException(HttpStatusCode code, string message)
        {
            switch (code)
            {
                case HttpStatusCode.NotFound:
                    var exceptionNotFound = JsonConvert.DeserializeObject<NotFoundException>(message);
                    throw (exceptionNotFound.ErrorType == BaseErrorType.NotFoundError)
                        ? exceptionNotFound
                        : new WebClientException(code, message);

                case HttpStatusCode.UnprocessableEntity:
                    var exceptionUnprocessableEntity = JsonConvert.DeserializeObject<ValidationException>(message);
                    throw (exceptionUnprocessableEntity.ErrorType == BaseErrorType.ValidationError)
                        ? exceptionUnprocessableEntity
                        : new WebClientException(code, message);

                default:
                    throw new WebClientException(code, message);
            }
        }
    }
}
