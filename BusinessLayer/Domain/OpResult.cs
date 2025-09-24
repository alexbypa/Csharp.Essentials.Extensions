namespace BusinessLayer.Domain;
public readonly struct OpResult<T> {
    public bool Success { get; }
    public T? Value { get; }
    public string? Error { get; }
    public string? IdTransaction { get; }

    private OpResult(bool success, T? value, string? error, string IdTransaction)
        => (Success, Value, Error) = (success, value, error);

    public static OpResult<T> Ok(T value, string IdTransaction) => new(true, value, null, IdTransaction);
    public static OpResult<T> Fail(string error, string IdTransaction) => new(false, default, error, IdTransaction);
}