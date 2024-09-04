/*
 * @Author: Jun
 * @Description:
 */

using System.Text.Json;

namespace MinimalApis;

public class ResponseWrapperMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        // 上下文端点元数据的获取， 判断是否不为空
        if (context.GetEndpoint()?.Metadata.GetMetadata<EnableResponseWrapperAttribute>() is not null)
        {
            // 保存原始响应流
            var originalResponseBodyStream = context.Response.Body;
            try
            {
                // 创建实例，在结束后自动释放
                using var memoryStream = new MemoryStream();
                // 替换Http响应输出流为memS，后续next操作将写入内存流而不是原始响应流
                context.Response.Body = memoryStream;
                // 调用next委托，调用管道中的下一个中间件或终结点
                await next(context);

                // 恢复原始响应流
                context.Response.Body = originalResponseBodyStream;

                // 将内存流的读取位置重置到流的开始处
                memoryStream.Seek(0, SeekOrigin.Begin);
                // 读取内存流中的内容，直到末尾
                var readToEnd = await new StreamReader(memoryStream).ReadToEndAsync();
                // 字符串反序列化对象
                var objResult = JsonSerializer.Deserialize<dynamic>(readToEnd);
                // 创建反序列化后的数据
                var result = new ResultModel<object>
                {
                    Data = objResult,
                    IsSuccess = true,
                    StatusCode = context.Response.StatusCode
                };
                // 将对象序列化成Json
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