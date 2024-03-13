using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using WebApi.Common.Exceptions;

namespace WebApi.Common
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;
        public ErrorHandlerMiddleware(RequestDelegate next,
            ILogger<ErrorHandlerMiddleware> logger,
            IConfiguration configuration)
        {
            _next = next;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                if (error is ValidationException validationException)
                {
                    await WriteValidationErrorResponse(validationException, response);
                    return;
                }
                if (error is AppBadRequestException badReqException)
                {
                    await WriteBadRequestResponse(badReqException, response);
                    return;
                }
                var allowErrorDetails = _configuration.GetValue<bool>("AllowErrorDetails");
                var showError = allowErrorDetails;// || context.User.Claims.Any(x => x.Type == ClaimTypes.Role && x.Value == Roles.SuperAdmin);

                response.StatusCode = error switch
                {
                    KeyNotFoundException e => (int)HttpStatusCode.NotFound,
                    _ => (int)HttpStatusCode.InternalServerError,
                };

                var resp = new
                {
                    Message = showError ? error?.Message : "Something went wrong please contact the system administrator.",
                    Details = showError ? error?.ToString() : null,
                };

                _logger.LogError(error, error?.Message);

                var result = JsonSerializer.Serialize(resp);
                await response.WriteAsync(result);
            }
        }

        private Task WriteBadRequestResponse(AppBadRequestException badReqException, HttpResponse response)
        {
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            var resp = new
            {
                badReqException.Message,
            };
            var result = JsonSerializer.Serialize(resp);
            return response.WriteAsync(result);
        }

        private static Task WriteValidationErrorResponse(ValidationException error, HttpResponse response)
        {
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            var resp = new
            {
                Message = "Validation Error",
                Errors = error.Errors.ToDictionary(x => x.PropertyName, x => x.ErrorMessage)
            };
            var result = JsonSerializer.Serialize(resp);
            return response.WriteAsync(result);
        }
    }
}
