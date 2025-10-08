#!/bin/bash

# ==========================================================
# VALIDATION FUNCTION
# ==========================================================
# Checks if an environment variable is empty.
# Args: $1 = Variable Name, $2 = JSON Key Description, $3 = JSON Key
check_required_env() {
  if [ -z "$1" ]; then
    echo "################################################################" >&2
    echo "### CRITICAL ERROR: Missing Environment Variable!            ###" >&2
    echo "################################################################" >&2
    echo "" >&2
    echo "The mandatory configuration for '$2' (Key: $3) has not been defined." >&2
    echo "" >&2
    echo "Use the Docker -e flag to pass it:" >&2
    echo "  e.g.: -e $3=\"correct_value\"" >&2
    echo "The container is stopping (exit 1)." >&2
    echo "" >&2
    exit 1
  fi
}

# ==========================================================
# EXECUTE VALIDATION
# ==========================================================
echo "Starting critical configuration validation..."
# 1. Validation for Database Provider (e.g.: postgresql or sqlserver)
check_required_env "$DatabaseProvider" "Database Provider" "DatabaseProvider"
check_required_env "$ConnectionStrings__Default" "Main Connection String" "ConnectionStrings__Default"

# 2. Validation for Telemetry options
check_required_env "$Serilog__SerilogConfiguration__LoggerTelemetryOptions__ConnectionString" "Telemetry Connection String" "Serilog__SerilogConfiguration__LoggerTelemetryOptions__ConnectionString"
check_required_env "$Serilog__SerilogConfiguration__LoggerTelemetryOptions__IsEnabled" "IdEnabled Telemetry" "Serilog__SerilogConfiguration__LoggerTelemetryOptions__IsEnabled"

# 3. Validation for MSSQL Log Sink Connection String
# The app requires this connection only if the DatabaseProvider is set to 'sqlserver'
if [ "$DatabaseProvider" = "sqlserver" ]; then
    check_required_env "$Serilog__SerilogOption__MSSqlServer__connectionString" "MSSQL Log Sink Connection String" "Serilog__SerilogOption__MSSqlServer__connectionString"
fi


echo "Validation completed. All critical configurations are present."
echo ""

# ==========================================================
# DEBUG BLOCK: PRINT ENVIRONMENT VARIABLES
# ==========================================================
echo "### DEBUG: ENVIRONMENT VARIABLES IN CONTAINER ###"
																																								
env | grep -E 'ASPNET|Connect|Serilog|Database' 
echo "##################################################"
echo ""

# ==========================================================
# STARTUP MESSAGE AND APPLICATION EXECUTION
# ==========================================================
echo "################################################################"
echo "##               STARTING .NET CORE DEMO WEB API              ##"
echo "################################################################"
echo ""
echo "Exposed Port: 8080 (map it to the host with -p <HOST_PORT>:8080)"
echo "API Documentation: http://localhost:<HOST_PORT>/scalar"
echo "Logger Dashboard: http://localhost:<HOST_PORT>/dashboard"
echo ""

# Start the .NET application
exec dotnet Web.Api.dll