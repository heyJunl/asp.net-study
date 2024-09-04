namespace MinimalApis;

public interface IResultModel
{
    bool? IsSuccess { get; }
    string? Message { get; }
    int? StatusCode { get; set; }
    long? Timestamp { get; }
    
}

public interface IResultModel<out T> : IResultModel
{
    T? Data { get; }
}