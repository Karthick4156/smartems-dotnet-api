namespace SmartEMS.API.Helpers;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public required string Message { get; set; }
    public required T Data { get; set; }
    public required List<string> Errors { get; set; }

    public static ApiResponse<T> SuccessResponse(T data, string message = "")
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data,
            Errors = new List<string>()
        };
    }

    public static ApiResponse<T> FailureResponse(List<string> errors, string message = "")
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Data = default!,
            Errors = errors
        };
    }
}