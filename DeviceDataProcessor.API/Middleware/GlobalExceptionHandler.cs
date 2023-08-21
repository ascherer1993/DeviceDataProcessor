using System.Net;
using DeviceDataProcessor.Models;
using Newtonsoft.Json;

namespace DeviceDataProcessor.Middleware;

public class GlobalExceptionHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
    {
        _next = next;
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (BasicException basicException)
        {
            await HandleExceptionAsync(httpContext, basicException);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        var response = context.Response;
        
        var responseBody = new BasicResponse
        {
            Success = false,
            Message = exception.Message
        };
        
        switch (exception)
        {
            case BasicException ex:
                response.StatusCode = (int) ex.HttpStatusCode;
                break;
            case ApplicationException:
                response.StatusCode = (int) HttpStatusCode.BadRequest;
                break;
            default:
                response.StatusCode = (int) HttpStatusCode.InternalServerError;
                responseBody.Message = "Internal server error";
                break;
        }
        _logger.LogError(exception.Message);
        var result = JsonConvert.SerializeObject(responseBody);
        await context.Response.WriteAsync(result);
    }
}