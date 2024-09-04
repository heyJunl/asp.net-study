/*
 * @Author: Jun
 * @Description:
 */

namespace MinimalApis;

public class ResultModel<T> : IResultModel<T>
{
    public ResultModel()
    {
        Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
    }

    /// <summary>
    ///     处理是否成功
    /// </summary>
    public bool? IsSuccess { get; set; }

    /// <summary>
    ///     错误信息
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    ///     业务码
    /// </summary>
    public int? StatusCode { get; set; }

    /// <summary>
    ///     时间戳
    /// </summary>
    public long? Timestamp { get; }

    /// <summary>
    ///     返回数据
    /// </summary>
    public T? Data { get; set; }


    /// <summary>
    ///     成功
    /// </summary>
    /// <param name="Data"></param>
    public ResultModel<T> Success(T? data = default)
    {
        this.IsSuccess = true;
        StatusCode = 200;
        Data = data;
        return this;
    }

    /// <summary>
    ///     失败
    /// </summary>
    /// <param name="msg">说明</param>
    /// <param name="code"></param>
    public ResultModel<T> Failed(string? msg = "failed", int? code = 500)
    {
        IsSuccess = false;
        Message = msg;
        StatusCode = code;
        return this;
    }
}

/// <summary>
///     返回结果
/// </summary>
public static class ResultModel
{
    /// <summary>
    ///     数据已存在
    /// </summary>
    /// <returns></returns>
    public static IResultModel<string> HasExists => Failed("data already exists");

    /// <summary>
    ///     数据不存在
    /// </summary>
    public static IResultModel<string> NotExists => Failed("data doesn't exist");

    /// <summary>
    ///     成功
    /// </summary>
    /// <param name="data">返回数据</param>
    /// <returns></returns>
    public static IResultModel<T> Success<T>(T? data = default)
    {
        return new ResultModel<T>().Success(data);
    }

    /// <summary>
    ///     成功
    /// </summary>
    /// <param name="task">任务</param>
    /// <returns></returns>
    public static async Task<IResultModel<T>> SuccessAsync<T>(Task<T>? task = default)
    {
        return task is not null && task != default ? new ResultModel<T>().Success(await task) : new ResultModel<T>();
    }

    /// <summary>
    ///     成功
    /// </summary>
    /// <returns></returns>
    public static IResultModel<string> Success()
    {
        return Success<string>();
    }


    /// <summary>
    ///     失败
    /// </summary>
    /// <param name="error">错误信息</param>
    /// <returns></returns>
    public static IResultModel<T> Failed<T>(string? error = null)
    {
        return new ResultModel<T>().Failed(error ?? "failed");
    }

    /// <summary>
    ///     失败
    /// </summary>
    /// <returns></returns>
    public static IResultModel<string> Failed(string? error = null)
    {
        return Failed<string>(error);
    }

    /// <summary>
    ///     根据布尔值返回结果
    /// </summary>
    /// <param name="success"></param>
    /// <returns></returns>
    public static IResultModel<T> Result<T>(bool success)
    {
        return success ? Success<T>() : Failed<T>();
    }

    /// <summary>
    ///     根据布尔值返回结果
    /// </summary>
    /// <param name="success"></param>
    /// <returns></returns>
    public static async Task<IResultModel> Result(Task<bool> success)
    {
        return await success ? Success() : Failed();
    }

    /// <summary>
    ///     根据布尔值返回结果
    /// </summary>
    /// <param name="success"></param>
    /// <returns></returns>
    public static IResultModel<string> Result(bool success)
    {
        return success ? Success() : Failed();
    }

    /// <summary>
    /// 时间戳起始日期
    /// </summary>
    public static readonly DateTime TimestampStart = new(1970, 1, 1, 0, 0, 0, 0);


}