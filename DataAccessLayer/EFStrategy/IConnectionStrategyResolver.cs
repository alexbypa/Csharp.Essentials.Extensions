namespace DataAccessLayer.EFStrategy;
public interface IConnectionStrategyResolver {
    IConnectionStrategy Resolve(string provider);
}