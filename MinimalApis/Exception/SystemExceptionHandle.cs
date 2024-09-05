/*
 * @Author: Jun
 * @Description:
 */

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MinimalApis.Exception;

/**
 * 处理系统异常
 */
public class SystemExceptionHandle(ILogger<CustomException> logger, IWebHostEnvironment environment): IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, System.Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is CustomException) return false;
        logger.LogError(exception, "Exception occurred: {Message} {StackTrace} {Source}", exception.Message,
            exception.StackTrace, exception.Source);

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An error occured while processing your request"
        };
        if (environment.IsDevelopment())
        {
            problemDetails.Detail =
                $"Exception occurred: {exception.Message} {exception.StackTrace} {exception.Source}";
        }

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}