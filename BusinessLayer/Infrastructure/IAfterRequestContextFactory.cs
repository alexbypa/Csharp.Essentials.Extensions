using BusinessLayer.Domain;
using BusinessLayer.Contracts;

namespace BusinessLayer.Infrastructure;
public interface IAfterRequestContextFactory {
    AfterRequestContext Create(
        HttpRequestSpec spec,
        HttpRequestMessage req,
        HttpResponseMessage res,
        int retry,
        TimeSpan elapsed);
}