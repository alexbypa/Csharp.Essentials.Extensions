Ecco un **documento Markdown unificato** che combina i concetti di `AddRequestAction`, `addRetryCondition` e l'uso dei **mock HTTP**, con esempi pratici e una struttura chiara per un pubblico internazionale.

---

# **🚀 Advanced HTTP Handling with CSharpEssentials.HttpHelper**
*Custom Actions, Retry Logic, and Mock Testing*

---

## **📌 Overview**
`CSharpEssentials.HttpHelper` provides powerful tools for:
1. **Custom Request Actions** (`AddRequestAction`) – Intercept and log/modify requests/responses.
2. **Automatic Retries** (`addRetryCondition`) – Handle transient failures gracefully.
3. **HTTP Mocking** – Simulate APIs for testing without real endpoints.

---

## **1️⃣ `AddRequestAction`: Custom Logging & Modifications**
### **🔧 Syntax**
```csharp
IhttpsClientHelper AddRequestAction(
    Func<HttpRequestMessage, HttpResponseMessage, int, TimeSpan, Task> action
);
```
| Parameter          | Type               | Description                                                                 |
|--------------------|--------------------|-----------------------------------------------------------------------------|
| `action`           | `Func<..., Task>`  | Async callback after each request.                                         |
| `HttpRequestMessage` | Request object    | Inspect/modify the request (URL, headers, method).                         |
| `HttpResponseMessage`| Response object   | Inspect/modify the response (status, content, headers).                   |
| `retryCount`       | `int`              | Number of retries attempted (if `addRetryCondition` is configured).        |
| `elapsed`          | `TimeSpan`         | Time spent due to rate limiting (if enabled).                              |

---

### **💡 Example: Logging & Metrics**
```csharp
app.MapGet("/api/test-logging", async (IhttpsClientHelperFactory httpFactory) =>
{
    var client = httpFactory.CreateClient("httpbin")
        .AddRequestAction(async (req, res, retry, elapsed) =>
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[LOG] {req.Method} {req.RequestUri} → {res.StatusCode}");
            Console.WriteLine($"     Retry: {retry}, RateLimit Delay: {elapsed.TotalMilliseconds}ms");
            Console.ResetColor();
            await Task.CompletedTask;
        });

    var response = await client.SendAsync(
        "https://httpbin.org/get",
        HttpMethod.Get,
        body: null,
        contentBuilder: new NoBodyContentBuilder()
    );

    return Results.Ok(await response.Content.ReadAsStringAsync());
});
```
**Output**:
```
[LOG] GET https://httpbin.org/get → OK
     Retry: 0, RateLimit Delay: 0ms
```

---

### **🔍 Implementation Details**
- **Thread Safety**: Callbacks run via `Task.WhenAll` (non-blocking).
- **Use Cases**:
  - **A/B Testing**: Inject headers dynamically.
  - **Error Tracking**: Log failed requests to monitoring tools.
  - **Response Modification**: Add custom headers (e.g., `X-Request-ID`).

---

## **2️⃣ `addRetryCondition`: Automatic Retries**
### **🔧 Syntax**
```csharp
void addRetryCondition(
    Func<HttpResponseMessage, bool> shouldRetry,
    int maxRetries,
    double backoffFactor
);
```
| Parameter       | Type               | Description                                  |
|-----------------|--------------------|----------------------------------------------|
| `shouldRetry`   | `Func<..., bool>`  | Condition to trigger a retry (e.g., `500` errors). |
| `maxRetries`    | `int`              | Maximum retry attempts.                     |
| `backoffFactor` | `double`           | Delay multiplier between retries (exponential backoff). |

---

### **💡 Example: Retry on Timeout**
```csharp
app.MapGet("/api/test-retry", async (IhttpsClientHelperFactory httpFactory) =>
{
    var client = httpFactory.CreateClient()
        .addTimeout(TimeSpan.FromSeconds(10)) // Timeout after 10s
        .AddRequestAction(async (req, res, retry, elapsed) =>
        {
            Console.WriteLine($"[RETRY] Attempt {retry}: {res.StatusCode}");
        })
        .addRetryCondition(
            res => res.StatusCode == HttpStatusCode.RequestTimeout, // Retry on timeout
            maxRetries: 2,
            backoffFactor: 2.0 // 1s → 2s → 4s delays
        );

    var response = await client.SendAsync(
        "https://myfakesiteOnError.com/",
        HttpMethod.Get,
        contentBuilder: new NoBodyContentBuilder()
    );

    return Results.Ok(await response.Content.ReadAsStringAsync());
});
```
**Output**:
```
[RETRY] Attempt 0: RequestTimeout
[RETRY] Attempt 1: OK
```

---

### **🔍 How Retries Work**
1. **First Attempt**: Fails with `504 RequestTimeout`.
2. **Retry Logic**: Checks `shouldRetry` → triggers retry after `backoffFactor` delay.
3. **Second Attempt**: Succeeds with `200 OK`.

> **⚠️ Note**: Ensure the mock’s delay is **shorter** than the client’s timeout (e.g., mock delays 5s, client timeout 10s).

---

## **3️⃣ HTTP Mocking: Simulate APIs**
### **🎭 Overview**
Mock HTTP responses for testing without real APIs. Use cases:
- **Timeout Simulation**: Test retry logic.
- **Error Scenarios**: Simulate `500`, `401`, etc.
- **Sequential Responses**: First call fails, second succeeds.

---

### **💡 Example: Mock Timeout → Success**
```csharp
public class HttpMockLibraryTimeoutThenOk : IHttpMockScenario
{
    private int _callCount = 0;

    public Func<HttpRequestMessage, bool> Match => req =>
        req.RequestUri?.AbsoluteUri.Contains("myfakesiteOnError") == true;

    public IReadOnlyList<Func<Task<HttpResponseMessage>>> ResponseFactory => new List<Func<Task<HttpResponseMessage>>>
    {
        async () => {
            if (_callCount++ == 0)
            {
                await Task.Delay(2000); // Simulate slow response
                return new HttpResponseMessage(HttpStatusCode.RequestTimeout)
                {
                    Content = new StringContent("Simulated timeout")
                };
            }
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Success after retry")
            };
        }
    };
}
```

**Registration** (in `Program.cs`):
```csharp
builder.Services.AddTransient<IHttpMockScenario, HttpMockLibraryTimeoutThenOk>();
builder.Services.AddTransient<IHttpMockEngine, HttpMockEngine>();
```

---

### **🔍 Mock Workflow**
1. **First Call**: Mock returns `504 RequestTimeout` after 2s.
2. **Retry**: Client retries (due to `addRetryCondition`).
3. **Second Call**: Mock returns `200 OK`.

> **⚠️ Critical**: Use `AddTransient` for mock scenarios to ensure **fresh state per request**.

---

## **4️⃣ Full Demo: Retry + Mock + Logging**
### **📌 Combined Example**
```csharp
app.MapGet("/api/demo-all", async (IhttpsClientHelperFactory httpFactory) =>
{
    var client = httpFactory.CreateClient()
        // 1. Configure retry on timeout
        .addRetryCondition(
            res => res.StatusCode == HttpStatusCode.RequestTimeout,
            maxRetries: 2,
            backoffFactor: 1.5
        )
        // 2. Add logging
        .AddRequestAction(async (req, res, retry, elapsed) =>
        {
            Console.WriteLine($@"[DEMO]
                URL:      {req.RequestUri}
                Status:   {res.StatusCode}
                Retry:    {retry}
                Delay:    {elapsed.TotalMilliseconds}ms");
        })
        // 3. Set timeout (shorter than mock delay to trigger retry)
        .addTimeout(TimeSpan.FromSeconds(3));

    // 4. Call the mocked endpoint
    var response = await client.SendAsync(
        "https://myfakesiteOnError.com/",
        HttpMethod.Get,
        contentBuilder: new NoBodyContentBuilder()
    );

    return Results.Ok(await response.Content.ReadAsStringAsync());
});
```

**Expected Output**:
```
[DEMO]
    URL:      https://myfakesiteOnError.com/
    Status:   RequestTimeout
    Retry:    0
    Delay:    0ms
[DEMO]
    URL:      https://myfakesiteOnError.com/
    Status:   OK
    Retry:    1
    Delay:    1500ms  // Backoff delay (1.5s)
```

---

## **🌍 Best Practices**
| Feature               | Recommendation                                                                 |
|-----------------------|-------------------------------------------------------------------------------|
| **Logging**           | Use structured logging (e.g., Serilog) in production.                       |
| **Retries**           | Limit `maxRetries` (e.g., 3) to avoid cascading failures.                    |
| **Mocks**             | Register as `Transient` to avoid shared state.                              |
| **Timeouts**          | Set client timeout **longer** than mock delays.                              |
| **Error Handling**    | Wrap callbacks in `try-catch` to prevent pipeline crashes.                  |

---