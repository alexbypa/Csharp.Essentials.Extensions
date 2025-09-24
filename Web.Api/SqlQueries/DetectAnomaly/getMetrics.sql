select M.*, T."TagsJson" "TraceJson" from "MetricEntry" M 
	inner join "TraceEntry" T ON M."TraceId" = T."TraceId"
	where M."TraceId" IS NOT NULL and M."Name" = 'db.client.commands.duration' 
		order by M."Timestamp" desc limit 100