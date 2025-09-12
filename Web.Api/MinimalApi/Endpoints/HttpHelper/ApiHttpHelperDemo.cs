using Azure.Identity;
using BusinessLayer.Adapters;
using BusinessLayer.Domain;
using BusinessLayer.Ports;
using BusinessLayer.UseCases;
using CSharpEssentials.HttpHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Microsoft.OpenApi.Validations;
using Scalar.AspNetCore;
using System;
using System.Net;
using System.Numerics;
using HttpMethod = System.Net.Http.HttpMethod;

namespace Web.Api.MinimalApi.Endpoints.HttpHelper;
public class ApiHttpHelperDemo : IEndpointDefinition {
    public void DefineEndpoints(WebApplication app) {
        IContentBuilder contentBuilder = new NoBodyContentBuilder();
        HttpHelperDemo demo = new HttpHelperDemo();

        //Minimal simple example of HttpHelper usage
        app.MapGet("Test/echo", async (string httpOptionName, IhttpsClientHelperFactory httpFactory) => {
            var spec = new HttpRequestSpec {
                Url = "http://www.yoursite.com/echo",
                Method = "POST",
                Body = "{'name':'Request','value':'Simple'}"
            };

            var ctxFactory = new DefaultAfterRequestContextFactory();
            var netClient = new HttpHelperNetworkClient(httpFactory, ctxFactory, null);

            var result = await new FetchAndLogUseCase(netClient).ExecuteAsync(httpOptionName, spec, new NoBodyContentBuilder());
            return result.Success ? Results.Ok(result.Value) : Results.Problem(result.Error);
        })
        .WithTags("HttpHelper")
        .WithSummary("Simple Request")
        .WithDescription("""This is first use of HttpHelper !""");

        //Example of HttpHelper with AddRequestAction
        app.MapGet("Test/AddAction", async ([FromQuery] string httpOptionName, IhttpsClientHelperFactory httpFactory) => {
            var spec = new HttpRequestSpec {
                Url = "http://www.yoursite.com/retry",
                Method = "POST",
                Body = "{'name':'Request','value':'Simple'}"
            };

            var ctxFactory = new DefaultAfterRequestContextFactory();


            // 2) azioni "on demand"(puoi metterne 0..N)
            var actions = new IRequestAction[]{
                new ConsoleColorRequestAction(),
                new InlineRequestAction(ctx =>
                {
                    Console.WriteLine($"[INLINE] {ctx.Method} {ctx.Url} -> {ctx.StatusCode} in {ctx.Elapsed.TotalMilliseconds} ms");
                    return Task.CompletedTask;
                })
            };

            var netClient = new HttpHelperNetworkClient(httpFactory, ctxFactory, null, actions);

            var result = await new FetchAndLogUseCase(netClient).ExecuteAsync(httpOptionName, spec, new NoBodyContentBuilder());
            return result.Success ? Results.Ok(result.Value) : Results.Problem(result.Error);
        })
        .WithSummary("Add Action")
        .WithTags("HttpHelper")
        .WithDescription("Add a custom action to log the status code of the response and other info.");

        app.MapGet("Test/auth/check", async (
            [FromServices] IhttpsClientHelperFactory httpFactory,
            [FromQuery] string httpOptionName = "Test_No_RateLimit",
            [FromQuery] string token = "super-secret",
            [FromQuery] string Username = "Username",
            [FromQuery] string Password = "Password"
            ) => {
                var spec = new HttpRequestSpec {
                    Url = "http://www.yoursite.com/auth/check",
                    Method = "POST",
                    Body = "{'name':'Request','value':'Simple'}",
                    Timeout = TimeSpan.FromSeconds(30),
                    Headers = new Dictionary<string, string> { { "mode", "Test" } },
                    Auth = new HttpAuthSpec { BearerToken = token, BasicUsername = Username, BasicPassword = Password }
                };

                var ctxFactory = new DefaultAfterRequestContextFactory();

                IHttpClientStep[] steps = [
                    new TimeoutStep(),
                new HeadersAndBearerStep()
                ];

                var actions = new IRequestAction[]{
                new ConsoleColorRequestAction(),
                new InlineRequestAction(ctx =>
                {
                    Console.WriteLine($"[INLINE] {ctx.Method} {ctx.Url} -> {ctx.StatusCode} in {ctx.Elapsed.TotalMilliseconds} ms");
                    return Task.CompletedTask;
                })
            };

                var netClient = new HttpHelperNetworkClient(httpFactory, ctxFactory, steps, actions);


                var result = await new FetchAndLogUseCase(netClient).ExecuteAsync(httpOptionName, spec, new NoBodyContentBuilder());
                return result.Success ? Results.Ok(result.Value) : Results.Problem(result.Error);
            })
        .WithTags("HttpHelper")
        .WithSummary("Bearer token check (demo)")
        .WithDescription("""
        Returns **200 OK** when `token` equals `super-secret`.

        Returns **401 Unauthorized** otherwise (or if omitted).
        """);

        app.MapGet("Test/retry", async (
            IhttpsClientHelperFactory httpFactory,
            [FromQuery] string httpOptionName = "Test_No_RateLimit",
            [FromQuery] int retries = 3,
            [FromQuery] double backofFactoryDelay = 2
            ) => {
                var spec = new HttpRequestSpec {
                    Url = "http://www.yoursite.com/retry",
                    Method = "POST",
                    Body = "{'name':'Request','value':'Simple'}",
                    Timeout = TimeSpan.FromSeconds(30),
                    Headers = new Dictionary<string, string> { { "mode", "Test" } }
                };

                var ctxFactory = new DefaultAfterRequestContextFactory();

                IHttpClientStep[] steps = [
                    new TimeoutStep(),
                new RetryConditionStep(
                    res => res.StatusCode == HttpStatusCode.InternalServerError,
                    retries,
                    backofFactoryDelay
                )
                ];

                var netClient = new HttpHelperNetworkClient(httpFactory, ctxFactory, steps);

                var result = await new FetchAndLogUseCase(netClient).ExecuteAsync(httpOptionName, spec, new NoBodyContentBuilder());
                return result.Success ? Results.Ok(result.Value) : Results.Problem(result.Error);
            }).WithSummary("Test Retry")
          .WithTags("HttpHelper");

        app.MapGet("Test/testtimeout", async (
            IhttpsClientHelperFactory httpFactory,
            [FromQuery] string httpOptionName = "Test_No_RateLimit"
            ) => {
                var spec = new HttpRequestSpec {
                    Url = "http://www.yoursite.com/timeout",
                    Method = "POST",
                    Body = "{'name':'Request','value':'Simple'}",
                    Timeout = TimeSpan.FromSeconds(1),
                    Headers = new Dictionary<string, string> { { "mode", "Test" } }
                };

                var ctxFactory = new DefaultAfterRequestContextFactory();

                IHttpClientStep[] steps = [
                    new TimeoutStep()
                ];

                var netClient = new HttpHelperNetworkClient(httpFactory, ctxFactory, steps);

                var result = await new FetchAndLogUseCase(netClient).ExecuteAsync(httpOptionName, spec, new NoBodyContentBuilder());
                return result.Success ? Results.Ok(result.Value) : Results.Problem(result.Error);
            }).WithSummary("Test Timeout")
          .WithTags("HttpHelper");

        app.MapGet("Test/testratelimit", async (
            IhttpsClientHelperFactory httpFactory,
            [FromQuery] string httpOptionName = "Test_With_RateLimit"
            ) => {
                var spec = new HttpRequestSpec {
                    Url = $"http://www.yoursite.com/users/get",
                    Method = "GET",
                    Body = "{'name':'Request','value':'Simple'}",
                    Headers = new Dictionary<string, string> { { "mode", "Test" } }
                };

                var ctxFactory = new DefaultAfterRequestContextFactory();

                var netClient = new HttpHelperNetworkClient(httpFactory, ctxFactory);

                Dictionary<string, OpResult<HttpResponseSpec>> results = new Dictionary<string, OpResult<HttpResponseSpec>>();

                for (int i = 1; i <= 10; i++) {
                    var result = await new FetchAndLogUseCase(netClient).ExecuteAsync(httpOptionName, spec, new NoBodyContentBuilder());
                    results.Add($"Step {i}", result);
                }
                return Results.Ok(results);
            }).WithSummary("Test Rate Limit")
          .WithTags("HttpHelper");
    }
}