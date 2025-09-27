select
	M."TraceId", 
	M."Value", 
	T."TagsJson" "TraceJson" from "MetricEntry" M 
	inner join "TraceEntry" T ON M."TraceId" = T."TraceId"
	where M."TraceId" IS NOT NULL and M."Name" = 'db.client.commands.duration' 
		and "Timestamp" between '2025-09-20T08:00:00' and '2025-09-28T11:00:00'
		order by M."Value" desc limit 10