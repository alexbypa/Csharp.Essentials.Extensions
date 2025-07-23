namespace DataAccessLayer.EFStrategy;
public class ConnectionStrategyResolver : IConnectionStrategyResolver {
    public IConnectionStrategy Resolve(string provider) => provider.ToLower() switch {
        "sqlserver" => new SqlServerStrategy(),
        "postgresql" => new PostgreSqlStrategy(),
        _ => throw new NotSupportedException($"Provider '{provider}' not supported.")
    };
}