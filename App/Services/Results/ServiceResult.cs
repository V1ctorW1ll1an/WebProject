namespace App.Services.Results;

public class ServiceResult<T>
{
    public bool IsSuccess { get; }
    public T Value { get; } = default!;
    public string ErrorMessage { get; } = string.Empty;

    private ServiceResult(T value)
    {
        Value = value;
        IsSuccess = true;
    }

    private ServiceResult(string errorMessage)
    {
        ErrorMessage = errorMessage;
        IsSuccess = false;
    }

    public static ServiceResult<T> Success(T value)
    {
        return new ServiceResult<T>(value);
    }

    public static ServiceResult<T> Failure(string error)
    {
        return new ServiceResult<T>(error);
    }
}
