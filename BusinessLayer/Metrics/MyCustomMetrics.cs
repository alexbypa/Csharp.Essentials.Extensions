using System.Collections.Concurrent;


namespace Web.Api.MinimalApi.Endpoints.Telemetries;

public sealed class MyCustomMetrics {
    private readonly TimeSpan _window = TimeSpan.FromMinutes(5);
    private readonly double _thresholdMs; // T (p95 target), es. 500
    private readonly ConcurrentQueue<(long ts, bool ok, int lat)> _q = new();

    public MyCustomMetrics(int thresholdLatencyMs = 500) => _thresholdMs = thresholdLatencyMs;

    public void Track(int statusCode, int latencyMs) {
        var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var ok = statusCode < 500;
        _q.Enqueue((now, ok, latencyMs));
        TrimOld(now);
    }

    public QosSnapshot Snapshot() {
        var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        TrimOld(now);

        var arr = _q.ToArray();
        if (arr.Length == 0)
            return new QosSnapshot(0, double.NaN, double.NaN, 0, (int)_thresholdMs);

        var count = arr.Length;
        var ok = arr.Count(x => x.ok);
        var sr = (double)ok / count;

        var p95 = Percentile(arr.Select(x => (double)x.lat).OrderBy(x => x).ToArray(), 0.95);
        var penalty = Math.Exp(-Math.Max(0.0, p95 - _thresholdMs) / (0.5 * _thresholdMs));
        var qos = (int)Math.Round(Math.Clamp(100.0 * sr * penalty, 0.0, 100.0));

        return new QosSnapshot(qos, sr * 100.0, p95, count, (int)_thresholdMs);
    }

    private void TrimOld(long nowMs) {
        var min = nowMs - (long)_window.TotalMilliseconds;
        while (_q.TryPeek(out var h) && h.ts < min)
            _q.TryDequeue(out _);
    }

    private static double Percentile(double[] sorted, double p) {
        if (sorted.Length == 0)
            return double.NaN;
        var idx = (int)Math.Ceiling(p * sorted.Length) - 1;
        return sorted[Math.Clamp(idx, 0, sorted.Length - 1)];
    }
}

public record QosSnapshot(
    int qos100,
    double success_rate_pct,
    double p95_ms,
    int count,
    int threshold_ms
);
