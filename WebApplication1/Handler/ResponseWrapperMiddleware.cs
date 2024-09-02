/*
 * @Author: Jun
 * @Description:
 */

using Newtonsoft.Json;
using WebApplication1.Common;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace WebApplication1.Handler;

public class ResponseWrapperMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.GetEndpoint()?.Metadata.GetMetadata<EnableResponseWrapperAttribute>() is not null)
        {
            var originalResponseBodyStream = context.Response.Body;
            try
            {
                using var memoryStream = new MemoryStream();
                context.Response.Body = memoryStream;

                await next(context);
                context.Response.Body = originalResponseBodyStream;

                memoryStream.Seek(0, SeekOrigin.Begin);
                var readToEnd = await new StreamReader(memoryStream).ReadToEndAsync();
                var objResult = JsonSerializer.Deserialize<dynamic>(readToEnd);
                var result = new ResultModel<object>
                {
                    Data = objResult,
                    IsSuccess = true,
                    StatusCode = context.Response.StatusCode
                };
                await context.Response.WriteAsJsonAsync(result as object);
            }
            finally
            {
                context.Response.Body = originalResponseBodyStream;
            }
        }
        else
        {
            await next(context);
        }
    }
}