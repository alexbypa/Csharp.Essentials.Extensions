namespace BusinessLayer.Domain;
public class OpResult<T> {
    public bool Success { get; set; }
    public T? Value { get; set; }
    public string? Error { get; set; }
    public string? IdTransaction { get; set; }

    //private OpResult(bool success, T? value, string? error, string IdTransaction)
    //    => (Success, Value, Error, IdTransaction) = (success, value, error, IdTransaction);

    public OpResult() { }

    public static OpResult<T> Ok(T value, string IdTransaction) {
        return new OpResult<T>() {
            Error = null,
            Value = value,
            Success = true,
            IdTransaction = IdTransaction
        };
    }
    public static OpResult<T> Fail(string error, string IdTransaction) {
        return new OpResult<T>() {
            Error = error,
            Value = default,
            Success = false,
            IdTransaction = IdTransaction
        };
    }
}