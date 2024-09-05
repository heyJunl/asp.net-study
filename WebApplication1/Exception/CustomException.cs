/*
 * @Author: Jun
 * @Description:
 */

namespace MinimalApis.Exception;

// 集成Exception将message传入
public class CustomException(int code, string message): System.Exception(message)
{
 public int Code { get; private set; } = code;
 public string Message { get; private set; } = message;

}