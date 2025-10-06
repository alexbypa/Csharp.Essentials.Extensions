#!/bin/bash

# ==========================================================
# FUNZIONE DI VALIDAZIONE
# ==========================================================
# Controlla se una variabile d'ambiente è vuota.
# Argomenti: $1 = Nome Variabile, $2 = Descrizione Chiave JSON
check_required_env() {
  if [ -z "$1" ]; then
    echo "################################################################" >&2
    echo "### ERRORE CRITICO: Variabile d'Ambiente mancante!           ###" >&2
    echo "################################################################" >&2
    echo "" >&2
    echo "La configurazione obbligatoria per '$2' (Chiave: $3) non è stata definita." >&2
    echo "" >&2
    echo "Utilizza il flag -e di Docker per passarla:" >&2
    echo "  es: -e $3=\"valore_corretto\"" >&2
    echo "Il container si sta arrestando (exit 1)." >&2
    echo "" >&2
    exit 1
  fi
}

# ==========================================================
# ESECUZIONE DELLA VALIDAZIONE
# ==========================================================
echo "Inizio validazione configurazioni critiche..."
# 1. Validazione del Database Provider (Esempio: postgresql o sqlserver)
check_required_env "$DatabaseProvider" "Database Provider" "DatabaseProvider"
check_required_env "$ConnectionStrings__Default" "Stringa di Connessione Principale" "ConnectionStrings__Default"
# 2. Validazione Stringa di Connessione Principale

check_required_env "$Serilog__SerilogConfiguration__LoggerTelemetryOptions__ConnectionString" "Stringa di Connessione Telemetry" "Serilog__SerilogConfiguration__LoggerTelemetryOptions__ConnectionString"

check_required_env "$Serilog__SerilogConfiguration__LoggerTelemetryOptions__IsEnabled" "IdEnabled Telemetry" "Serilog__SerilogConfiguration__LoggerTelemetryOptions__IsEnabled"

# 3. Validazione Stringa di Connessione per il Sink di Log MSSqlServer


echo "Validazione completata. Tutte le configurazioni critiche sono presenti."
echo ""

# ==========================================================
# NUOVO BLOCCO DEBUG: STAMPA TUTTE LE VARIABILI D'AMBIENTE
# ==========================================================
echo "### DEBUG: VARIABILI D'AMBIENTE NEL CONTAINER ###"
# 'env' elenca tutte le variabili. 'grep' filtra solo quelle che ci interessano.
env | grep -E 'ASPNET|Connect|Serilog|Database' 
echo "##################################################"
echo ""

# ==========================================================
# MESSAGGIO DI AVVIO E ESECUZIONE APPLICAZIONE
# ==========================================================
echo "################################################################"
echo "##               AVVIO DEMO WEB API .NET CORE                 ##"
echo "################################################################"
echo ""
echo "Porta esposta: 8080 (mappala all'host con -p <PORTA_HOST>:8080)"
echo "Documentazione API: http://localhost:<PORTA_HOST>/scalar"
echo "Dashboard Logger: http://localhost:<PORTA_HOST>/dashboard"
echo ""

# Avvio dell'applicazione .NET
exec dotnet Web.Api.dll