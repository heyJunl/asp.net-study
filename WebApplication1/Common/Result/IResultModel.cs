namespace WebApplication1.Common;

public interface IResultModel
{
    // 是否成功
    bool? IsSuccess { get; }
    
    // 错误信息
    string? Message { get; }
    
    // 状态码
    int? StatusCode { get; set; }
    
    // 时间戳
    long? Timestamp { get; }
}

/**
 * 多写一个接口的的好处是在做异常处理时可以不返回Data属性。
 * 这里的out作用时协变（Covariance）的关键字，允许子类返回父类类型的实例。
 * 由于协变，只能从接口返回T类型的值，而不能传递T类型的值給接口，只Get不Set
 */
public interface IResultModel<out T> : IResultModel
{
    T? Data { get; }
}