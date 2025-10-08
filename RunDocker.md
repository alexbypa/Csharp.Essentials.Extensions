# 🚀 Running CSharpEssentials.LoggerHelper Demo API
... (Omissis: Sezioni 1 e 1.1 - Base Configuration)

---

## 2. Testing Specific Sinks and Features

To activate any specific Sink, you must override the configuration defined in `appsettings.LoggerHelper.json`.

### 🚨 Important: Configuring Arrays (Levels, Conditions)

In Docker/Kubernetes, you cannot set individual array elements by index directly. To configure the log levels for a Sink, you must **override the entire array** using sequential indices in your environment variables.

| JSON Key | Environment Variable Pattern | Example Value |
| :--- | :--- | :--- |
| `Serilog:SerilogCondition:0:Level:0` | `Serilog__SerilogCondition__0__Level__0` | `Information` |
| `Serilog:SerilogCondition:0:Level:1` | `Serilog__SerilogCondition__0__Level__1` | `Error` |
| `Serilog:SerilogCondition:0:Level:2` | `Serilog__SerilogCondition__0__Level__2` | `Fatal` |

... (Omissis: Sezioni 1 e 2 - Testing Specific Sinks and Features introduction)

### 2.1. Scenario A: Enabling MSSQL Server Sink (Database Sink)

**Prerequisite:** Ensure `DatabaseProvider=sqlserver` in your `.env` and provide the required connection strings. The entrypoint script will validate the essential connection strings.

1.  **Critical Configuration Variables:**
    You must provide both the **log levels** and the **specific connection string for the Sink** (which overrides the `XXXXXXXXXXXXXXX` placeholder in the default configuration).

| Variable Key | Purpose | Example Value |
| :--- | :--- | :--- |
| `Serilog__SerilogConfiguration__SerilogCondition__0__Sink` | Activates MSSQL Sink at index 0. | `MSSqlServer` |
| `Serilog__SerilogCondition__0__Level__0` | Enables `Information` level logging. | `Information` |
| `Serilog__SerilogCondition__0__Level__1` | Enables `Warning` level logging. | `Warning` |
| `Serilog__SerilogCondition__0__Level__2` | Enables `Error` level logging. | `Error` |
| `Serilog__SerilogCondition__0__Level__3` | Enables `Fatal` level logging. | `Fatal` |
| **`Serilog__SerilogOption__MSSqlServer__connectionString`** | **CRITICAL:** Overrides the Sink's connection string placeholder. | `Server=mssql-container,1433;Database=test_db;User Id=SA;Password=strong!password#123;TrustServerCertificate=True` |

2.  **Run Example (using Docker Compose `mssql` profile):**
    Ensure the above variables are set either in the `webapi` environment block in `docker-compose.yml` or passed via your shell/`.env`.

    ```yaml
    # Docker Compose: webapi service environment block
    environment:
      # ... variables from .env ...
      
      # 1. Override Sink and Levels
      - Serilog__SerilogConfiguration__SerilogCondition__0__Sink=MSSqlServer
      - Serilog__SerilogConfiguration__SerilogCondition__0__Level__0=Information
      - Serilog__SerilogConfiguration__SerilogCondition__0__Level__1=Warning
      - Serilog__SerilogConfiguration__SerilogCondition__0__Level__2=Error
      - Serilog__SerilogConfiguration__SerilogCondition__0__Level__3=Fatal

      # 2. Sink Connection String
      - Serilog__SerilogOption__MSSqlServer__connectionString="Server=mssql-container,1433;Database=test_db;User Id=SA;Password=strong!password#123;TrustServerCertificate=True"
    ```

3.  **Command:** `docker compose --profile mssql up -d`

---

### 2.2. Scenario B: Enabling PostgreSQL Sink (Database Sink)

The default configuration includes a placeholder for the PostgreSQL Sink at array index 3.

1.  **Critical Configuration Variables:**

| Variable Key | Purpose | Example Value |
| :--- | :--- | :--- |
| `Serilog__SerilogConfiguration__SerilogCondition__3__Sink` | Activates PostgreSQL Sink at index 3. | `PostgreSQL` |
| `Serilog__SerilogCondition__3__Level__0` | Enables `Information` level logging. | `Information` |
| `Serilog__SerilogOption__PostgreSQL__connectionString` | **CRITICAL:** Overrides the Sink's connection string placeholder. | `Host=postgres-container;Port=5432;Database=test_db;Username=root;Password=root` |

2.  **Run Example (using Docker Compose `postgres` profile):**

    ```yaml
    # Docker Compose: webapi service environment block
    environment:
      # ... variables from .env ...
      
      # 1. Override Sink and Levels
      - Serilog__SerilogConfiguration__SerilogCondition__3__Sink=PostgreSQL
      - Serilog__SerilogConfiguration__SerilogCondition__3__Level__0=Information
      - Serilog__SerilogConfiguration__SerilogCondition__3__Level__1=Warning
      
      # 2. Sink Connection String
      - Serilog__SerilogOption__PostgreSQL__connectionString="Host=postgres-container;Port=5432;Database=test_db;Username=root;Password=root"
    ```

3.  **Command:** `docker compose --profile postgres up -d`

---