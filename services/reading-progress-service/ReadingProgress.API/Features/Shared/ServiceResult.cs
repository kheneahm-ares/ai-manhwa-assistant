namespace ReadingProgress.API.Features.Shared
{
    public record ServiceResult<T>
    {
        public bool IsSuccess { get; init; }
        public T? Data { get; init; }
        public string? ErrorMessage { get; init; }
        public ErrorType ErrorType { get; init; }

        public static ServiceResult<T> Success(T data) => new() { IsSuccess = true, Data = data };
        public static ServiceResult<T> Failure(string message, ErrorType errorType) => new() { IsSuccess = false, ErrorMessage = message, ErrorType = errorType };
    }

    public record ServiceResult
    {
        public bool IsSuccess { get; init; }
        public string? ErrorMessage { get; init; }
        public ErrorType ErrorType { get; init; }

        public static ServiceResult Success() => new() { IsSuccess = true };
        public static ServiceResult Failure(string message, ErrorType errorType) => new() { IsSuccess = false, ErrorMessage = message, ErrorType = errorType };
    }

    public enum ErrorType
    {
        NotFound,
        BadRequest,
        Conflict,
        Unauthorized
    }
}
