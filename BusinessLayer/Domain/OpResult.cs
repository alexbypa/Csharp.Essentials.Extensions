namespace BusinessLayer.Domain;
public readonly struct OpResult<T> {
    public bool Success { get; }
    public T? Value { get; }
    public string? Error { get; }

    private OpResult(bool success, T? value, string? error)
        => (Success, Value, Error) = (success, value, error);

    public static OpResult<T> Ok(T value) => new(true, value, null);
    public static OpResult<T> Fail(string error) => new(false, default, error);
}