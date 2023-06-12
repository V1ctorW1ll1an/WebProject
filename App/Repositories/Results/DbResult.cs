namespace App.Repositories.Results;

public class DbResult<T>
{
    public bool IsSuccess { get; }
    public T Value { get; } = default!;

    private DbResult(bool isSuccess, T value)
    {
        IsSuccess = isSuccess;
        Value = value;
    }

    public static DbResult<T> Success(T value)
    {
        return new DbResult<T>(true, value);
    }

    public static DbResult<T> Failure()
    {
        return new DbResult<T>(false, default!);
    }
}
