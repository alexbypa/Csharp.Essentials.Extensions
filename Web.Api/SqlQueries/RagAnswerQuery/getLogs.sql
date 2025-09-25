select "Id", "ApplicationName" "App", "TimeStamp" "Ts", "LogEvent" "Message", "IdTransaction" "TraceId" from "LogEntry"  
where "TimeStamp" > @dtStart and "Message" like '%401%'
order by "Id" desc
limit @n
