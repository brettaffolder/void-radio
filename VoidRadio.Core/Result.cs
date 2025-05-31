namespace VoidRadio.Core;

public readonly struct Result<T>
{
    public bool IsError { get; }
    public Error? Error { get; }
    public T? Value { get; }

    private Result(Error error)
    {
        IsError = true;
        Error = error;
        Value = default;
    }

    private Result(T value)
    {
        IsError = false;
        Error = default;
        Value = value;
    }

    public static implicit operator Result<T>(Error error) => new Result<T>(error);
    public static implicit operator Result<T>(T value) => new Result<T>(value);
}

public readonly struct Result
{
    public static Result Failure => new Result(new Error());
    public static Result Success => new Result(true);

    public bool IsError { get; }
    public Error? Error { get; }
    public bool? Value { get; }

    private Result(Error error)
    {
        IsError = true;
        Error = error;
        Value = default;
    }

    private Result(bool value)
    {
        IsError = false;
        Error = default;
        Value = value;
    }

    public static implicit operator Result(Error error) => new Result(error);
    public static implicit operator Result(bool value) => new Result(value);
}

public readonly struct Error(string? message = "Unknown error.")
{
    public string Message { get; } = message ?? "Unknown error.";
}