namespace RentnRoll.Domain.Common;

public class Result
{
    public bool IsSuccess { get; }
    public List<Error> Errors { get; }

    public bool IsError => !IsSuccess;

    protected Result()
    {
        Errors = [];
        IsSuccess = true;
    }

    protected Result(List<Error> errors)
    {
        if (errors.Count == 0)
        {
            throw new ArgumentException(
                nameof(errors), "Error list cannot be empty.");
        }
        Errors = errors;
        IsSuccess = false;
    }

    public static Result Success() => new();

    public static Result Failure(List<Error> errors) => new(errors);

    public TResult Match<TResult>(
        Func<TResult> onSuccess,
        Func<List<Error>, TResult> onError)
    {
        return IsSuccess ? onSuccess() : onError(Errors);
    }
}

public sealed class Result<T> : Result
{
    public T? Value { get; }

    private Result(T value) : base()
    {
        Value = value;
    }

    private Result(List<Error> errors) : base(errors)
    {
        Value = default;
    }

    public static implicit operator Result<T>(T value) => new(value);
    public static implicit operator Result<T>(List<Error> errors)
        => new(errors);
    public static implicit operator Result<T>(Error error)
        => new(new List<Error> { error });

    public TResult Match<TResult>(
        Func<T, TResult> onSuccess,
        Func<List<Error>, TResult> onError)
    {
        return IsSuccess ? onSuccess(Value!) : onError(Errors);
    }
}