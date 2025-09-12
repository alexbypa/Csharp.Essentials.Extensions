namespace BusinessLayer.Contracts;
public interface IAppLogger {
    void Info(string message, object? context = null);
    void Warning(string message, object? context = null);
    void Error(string message, Exception? ex = null, object? context = null);
}