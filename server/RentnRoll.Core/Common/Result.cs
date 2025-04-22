namespace RentnRoll.Core.Common.Result;

public class Result
{
    public Error? Error { get; }
    public bool IsSuccess { get; }

    public bool IsError => !IsSuccess;

    protected Result()
    {
        IsSuccess = true;
        Error = null;
    }

    protected Result(Error error)
    {
        Error = error ?? throw new ArgumentNullException(
            nameof(error), "Error cannot be null.");
        IsSuccess = false;
    }

    public static Result Success() => new();

    public static Result Failure(Error error) => new(error);

    public TResult Match<TResult>(
        Func<TResult> onSuccess,
        Func<Error, TResult> onError)
    {
        return IsSuccess ? onSuccess() : onError(Error!);
    }
}

public sealed class Result<T> : Result
{
    public T? Value { get; }

    private Result(T value) : base()
    {
        Value = value;
    }

    private Result(Error error) : base(error)
    {
        Value = default;
    }

    public static implicit operator Result<T>(T value) => new(value);
    public static implicit operator Result<T>(Error error) => new(error);

    public TResult Match<TResult>(
        Func<T, TResult> onSuccess,
        Func<Error, TResult> onError)
    {
        return IsSuccess ? onSuccess(Value!) : onError(Error!);
    }
}