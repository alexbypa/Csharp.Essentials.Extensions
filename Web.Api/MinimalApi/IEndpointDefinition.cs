namespace Web.Api.MinimalApi;
public interface IEndpointDefinition {
    Task DefineEndpointsAsync(WebApplication app);
}