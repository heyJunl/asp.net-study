/*
 * @Author: Jun
 * @Description:
 */

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MinimalApis.Exception;

/**
 * 可注册多个异常处理分别处理不同类型异常，按默认注册顺序处理，返回true则处理异常，返回fasle则会跳到下一个ExceptionHandle，未处理异常在UseExceptionHandler中间件做最后处理
 * 集成IExceptionHandler
 */
public class CustomExceptionHandler(ILogger<CustomException> logger, IWebHostEnvironment environment)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, System.Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not CustomException customException) return false;
        logger.LogError(exception, "Exception occurred:{Message} {StackTrace} {Source}", exception.Message,
            exception.StackTrace, exception.Source);
        var problemDetails = new ProblemDetails
        {
            Status = customException.Code,
            Title = customException.Message
        };
        if (environment.IsDevelopment())
        {
            problemDetails.Detail =
                $"Exception occurred: {customException.Message} {customException.StackTrace} {customException.Source}";
        }

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}