using BusinessLayer.Domain;
using BusinessLayer.Ports;

namespace BusinessLayer.Adapters;
public interface IAfterRequestContextFactory {
    AfterRequestContext Create(
        HttpRequestSpec spec,
        HttpRequestMessage req,
        HttpResponseMessage res,
        int retry,
        TimeSpan elapsed);
}