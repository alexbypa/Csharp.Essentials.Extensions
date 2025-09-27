-- select Id, ApplicationName App, TimeStamp Ts, LogEvent Message, IdTransaction TraceId from [MetricEntry]  order by id desc

SELECT "Timestamp1" "Ts", "Id", "TraceId", 'Demo' "App", "TagsJson" "Message", "Value" "duration" FROM "MetricEntry" where "Timestamp" >= '2025-09-27T07:00:00' and "Name" = 'http.server.request.duration' order by "Value" desc limit 20

-- Which traceId had the highest duration ?
-- You are a telemetry expert and can spot anomalies in HTTP calls