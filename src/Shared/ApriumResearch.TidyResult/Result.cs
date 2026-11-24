namespace ApriumResearch.TidyResult;

/// <summary>
/// Base result class representing the result of an operation.
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class Result<T>
{
    private readonly T? _value;
    private readonly Error? _error;

    /// <summary>
    /// Success constructor.
    /// </summary>
    /// <param name="value"></param>
    private Result(T value)
    {
        IsSuccess = true;
        _value = value;
        _error = Error.None;
    }

    /// <summary>
    /// Failure constructor.
    /// </summary>
    /// <param name="error"></param>
    private Result(Error error)
    {
        IsSuccess = false;
        _value = default;
        _error = error;
    }

    public bool IsSuccess { get; }

    public static Result<T> Success(T value) => new(value);

    public static Result<T> Failure(Error error) => new(error);
}