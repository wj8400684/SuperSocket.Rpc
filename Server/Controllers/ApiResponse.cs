namespace Server.Controllers;


/// <summary>
/// 返回基类
/// </summary>
public class ApiResponse
{
    /// <summary>
    /// 是否成功
    /// </summary>
    public bool Successful { get; set; }

    /// <summary>
    /// 错误消息
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// 错误代码
    /// </summary>
    public int? ErrorCode { get; set; }
}

/// <summary>
/// 自定义响应对象
/// </summary>
/// <typeparam name="T"></typeparam>
public class ApiResponse<TContentResponse> : ApiResponse
{
    public ApiResponse()
    {
    }

    public ApiResponse(TContentResponse content)
    {
        Content = content;
    }

    public ApiResponse(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }

    /// <summary>
    /// 响应内容
    /// </summary>
    public TContentResponse? Content { get; set; }
}
