using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using REDChallenge.Domain.ErrorObjects;
using REDChallenge.Domain.Exceptions;

namespace REDChallenge.Application.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
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

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var errorMsg = new StandardErrorMessage();
            var code = StatusCodes.Status500InternalServerError; // 500 if unexpected

            if (ex is NotFoundException)
            {
                code = StatusCodes.Status404NotFound;
                errorMsg.Title = "Resource not found";

            }
            else if (ex is InvalidLoginAttemptException)
            {
                code = StatusCodes.Status401Unauthorized;
                errorMsg.Title = ex.Message;
            }
            else if (ex is FluentValidation.ValidationException)
            {
                var validationException = (FluentValidation.ValidationException)ex;
                code = StatusCodes.Status400BadRequest;
                errorMsg.Title = "Validation Error";
                errorMsg.InvalidParams = validationException.Errors.Select(e => new InvalidParameter
                {
                    Name = e.PropertyName,
                    Reason = e.ErrorMessage,
                });
            }

            var result = JsonConvert.SerializeObject(errorMsg);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = code;
            return context.Response.WriteAsync(result);
        }
    }
}
