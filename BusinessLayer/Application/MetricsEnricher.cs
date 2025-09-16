using CSharpEssentials.LoggerHelper;
using Serilog;

namespace BusinessLayer.Application;
public sealed class MetricsEnricher : IContextLogEnricher {
    public ILogger Enrich(ILogger logger, object? context) {
        // Tag ambientali standard (puoi aggiungerne altri)
        logger = logger
            .ForContext("tags.service", "minimal-api")
            .ForContext("tags.env", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "unknown");

        // Se l'endpoint ha passato un oggetto di context, proietta tutte le proprietà nel log
        if (context is { }) {
            var t = context.GetType();
            foreach (var p in t.GetProperties()) {
                var v = p.GetValue(context);
                if (v != null)
                    logger = logger.ForContext(p.Name, v, true);
            }
        }
        return logger;
    }
    public LoggerConfiguration Enrich(LoggerConfiguration cfg) =>
            cfg.Enrich.WithProperty("tags.service", "minimal-api")
                      .Enrich.WithProperty("tags.env", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "unknown");
}