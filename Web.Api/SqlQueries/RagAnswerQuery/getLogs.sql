select "Id", "ApplicationName" "App", "TimeStamp" "Ts", "LogEvent" "Message", "IdTransaction" "TraceId" from "LogEntry"  
where "TimeStamp" > @now
order by "Id" desc
limit @n
