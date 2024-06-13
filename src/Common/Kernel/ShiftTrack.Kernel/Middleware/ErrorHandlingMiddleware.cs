using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ShiftTrack.Kernel.Constants;
using System.Net;
using ShiftTrack.Kernel.Exceptions;
using ShiftTrack.Kernel.Models;

namespace ShiftTrack.Kernel.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError;

            string result;

            switch (exception)
            {
                // handle standard exceptions
                case KernelException standartException when (standartException is EntityNotFoundException):
                    {
                        result = JsonConvert.SerializeObject(new KernelExceptionModel
                        {
                            Code = standartException.Code,
                            ErrorMessage = standartException.ErrorMessage,
                            ErrorType = standartException.ErrorType
                        });

                        statusCode = standartException.Code;

                        break;
                    }

                case KernelException userAlreadyExistException when (userAlreadyExistException is UserAlreadyExistException):
                    {
                        result = JsonConvert.SerializeObject(new KernelExceptionModel
                        {
                            Code = userAlreadyExistException.Code,
                            ErrorMessage = userAlreadyExistException.ErrorMessage,
                            ErrorType = userAlreadyExistException.ErrorType
                        });

                        statusCode = userAlreadyExistException.Code;

                        break;
                    }

                // handle validation exceptions
                case ValidationException validationException:
                    {
                        result = JsonConvert.SerializeObject(new KernelValidationExceptionModel
                        {
                            Code = validationException.Code,
                            ErrorMessage = validationException.ErrorMessage,
                            ErrorType = validationException.ErrorType,
                            ValidationErrors = validationException.Errors
                        });

                        statusCode = validationException.Code;

                        break;
                    }

                // handle custom exceptions
                case CustomException customException:
                    {
                        result = JsonConvert.SerializeObject(new KernelExceptionModel
                        {
                            Code = customException.Code,
                            ErrorMessage = customException.ErrorMessage,
                            ErrorType = customException.ErrorType
                        });

                        statusCode = customException.Code;

                        break;
                    }

                // handle other exceptions
                default:
                    {
                        result = JsonConvert.SerializeObject(new KernelExceptionModel
                        {
                            Code = HttpStatusCode.InternalServerError,
                            ErrorMessage = exception.Message,
                            ErrorType = BaseErrorType.KernelError
                        });

                        break;
                    }
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(result);
        }
    }
}
