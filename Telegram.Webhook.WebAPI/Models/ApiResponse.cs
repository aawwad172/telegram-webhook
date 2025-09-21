namespace Telegram.Webhook.WebAPI.Models;

public class ApiResponse<T>
{
    public bool Success { get; init; }
    public T Data { get; init; }
    public string ErrorMessage { get; init; }
    public string ErrorCode { get; init; }

    private ApiResponse(
        bool success,
        T data,
        string errorMessage,
        string errorCode)
    {
        Success = success;
        ErrorMessage = errorMessage;
        ErrorCode = errorCode;
        Data = data;
    }

    // Factory methods
    public static ApiResponse<T> SuccessResponse(T response)
        => new(success: true, data: response, errorMessage: "Success", errorCode: "0");

    public static ApiResponse<T> ErrorResponse(string errorMessage, string errorCode)
        => new(success: false, data: default!, errorMessage: errorMessage, errorCode: errorCode);
}