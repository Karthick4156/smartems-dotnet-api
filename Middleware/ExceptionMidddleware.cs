using System.Net;
using System.Text.Json;
using SmartEMS.API.Helpers;
using SmartEMS.API.Exceptions;

namespace SmartEMS.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
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
            context.Response.ContentType = "application/json";

            int statusCode = 500;

            if (ex is BadRequestException)
                statusCode = 400;

            context.Response.StatusCode = statusCode;

            var response = new
            {
                success = false,
                message = ex.Message,
                data = (object?)null,
                errors = new[] { ex.Message }
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }

    private static Task HandleException(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = ApiResponse<string>.FailureResponse(
            new List<string> { ex.Message },
            "An error occurred"
        );

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}