using BusinessLayer.Ports;

namespace BusinessLayer.Adapters;
public sealed class ConsoleColorRequestAction : IRequestAction {
    public Task OnAfterRequestAsync(AfterRequestContext r) {
        var bg = Console.BackgroundColor;
        var fg = Console.ForegroundColor;
        Console.BackgroundColor = ConsoleColor.Blue;
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"[{r.Method}] {r.Url} -> {r.StatusCode} (retry:{r.RetryCount}, {r.Elapsed.TotalMilliseconds} ms)");
        Console.BackgroundColor = bg;
        Console.ForegroundColor = fg;
        return Task.CompletedTask;
    }
}
public sealed class InlineRequestAction : IRequestAction {
    private readonly Func<AfterRequestContext, Task> _fn;
    public InlineRequestAction(Func<AfterRequestContext, Task> fn) => _fn = fn;
    public Task OnAfterRequestAsync(AfterRequestContext ctx) => _fn(ctx);
}