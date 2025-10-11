# Fase 1: Build (ambiente di sviluppo con SDK)
# Usiamo l'immagine ufficiale .NET 9.0 SDK per compilare l'applicazione.
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia i file di progetto per il restore iniziale delle dipendenze (ottimizzazione della cache Docker)
COPY ["Web.Api/Web.Api.csproj", "Web.Api/"]
COPY ["BusinessLayer/BusinessLayer.csproj", "BusinessLayer/"]
COPY ["DataAccessLayer/DataAccessLayer.csproj", "DataAccessLayer/"]

COPY Web.Api/appsettings.LoggerHelper.json Web.Api/

# Copia tutti gli altri file della soluzione
COPY . .

# Ripristina le dipendenze e compila in modalità Release
WORKDIR "/src/Web.Api"
RUN dotnet restore "Web.Api.csproj"
RUN dotnet publish "Web.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# --- LOG versioni assembly CSharpEssentials*.dll (aggiungi questa RUN) ---
RUN dotnet new console -n ver -o /tmp/ver --force && \
    printf 'using System;using System.IO;using System.Reflection;class P{static void Main(){foreach(var f in Directory.GetFiles("/app/publish","CSharpEssentials*.dll")){var v=AssemblyName.GetAssemblyName(f).Version;Console.WriteLine(System.IO.Path.GetFileName(f)+" "+v);}}}\n' > /tmp/ver/Program.cs && \
    dotnet run --project /tmp/ver/ver.csproj > /cse-assemblies.log && \
    cat /cse-assemblies.log

#RUN dotnet list package --include-transitive > /packages.txt && cat /packages.txt
#RUN dotnet list .\Web.Api\Web.Api.csproj package --include-transitive | Out-File -Encoding UTF8 .\packages.log
RUN dotnet list Web.Api.csproj package --include-transitive > /packages.log


# Fase 2: Runtime (ambiente di produzione snello)
# Usiamo l'immagine .NET 9.0 ASP.NET runtime, che è molto più piccola e sicura.
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
# Espone la porta usata dall'applicazione (il tuo Program.cs non specifica, ma è lo standard ASP.NET Core)
EXPOSE 8080

COPY entrypoint.sh .
# NEW: Converti le terminazioni di riga da CRLF a LF (necessario se creato su Windows)
RUN sed -i 's/\r$//' entrypoint.sh  # <--- AGGIUNTA FONDAMENTALE
RUN chmod +x entrypoint.sh

# Copia i file pubblicati dalla fase di build
COPY --from=build /app/publish .
# Definisce il comando da eseguire all'avvio del container

COPY --from=build /cse-assemblies.log /cse-assemblies.log

ENTRYPOINT ["./entrypoint.sh"]