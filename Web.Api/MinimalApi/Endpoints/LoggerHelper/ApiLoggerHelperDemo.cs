﻿using BusinessLayer.Contracts.Context;
using CSharpEssentials.LoggerHelper;
using CSharpEssentials.LoggerHelper.InMemorySink;
using Serilog.Events;
namespace Web.Api.MinimalApi.Endpoints.LoggerHelper;
public class ApiLoggerHelperDemo : IEndpointDefinition {
    public void DefineEndpoints(WebApplication app) {
        app.MapGet("Logger/Sample", (string Action, string message, LogEventLevel logEventLevel) => {
            loggerExtension<RequestSample>
            .TraceSync(
                new RequestSample() { 
                    IdTransaction = Guid.NewGuid().ToString(), 
                    Action = Action 
                }, 
                logEventLevel, 
                null, 
                message);
            return Results.Ok("Log inserted");
        })
        .WithSummary("Simple Request")
        .WithTags("LoggerHelper");
        
        app.MapGet("Logger/LogOnDashboard", (string Action, string message, LogEventLevel logEventLevel) => {
            loggerExtension<RequestSample>
            .TraceDashBoardSync(
                new RequestSample() { 
                    IdTransaction = Guid.NewGuid().ToString(), 
                    Action = Action
                }, 
                logEventLevel, 
                null,
                message + "{test}", "NewValue");
            var logsOnDashboard = InMemoryDashboardSink.GetLogEvents();
            return Results.Ok(logsOnDashboard);
        })
        .WithSummary("Add Log on Dashboard")
        .WithTags("LoggerHelper");
    }
}