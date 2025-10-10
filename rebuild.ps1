# rebuild-loggerhelper.ps1
# Rebuild, push e redeploy del servizio loggerhelper
$ErrorActionPreference = 'Stop'

try {
    # 0) Variabili base
    Write-Host "[INFO] Set variabili..."
    $USER = "alexbypa"
    $IMAGE = "loggerhelper-api"
    $TAG = "1.0.18"
    $FULL = "$($USER)/$($IMAGE):$($TAG)"
    Write-Host "USER=$USER IMAGE=$IMAGE TAG=$TAG FULL=$FULL"
    Write-Host ""

    # 1) Imposta contesto Kubernetes
    Write-Host "[RUN ] kubectl config use-context docker-desktop"
    kubectl config use-context docker-desktop
    Write-Host "[RUN ] kubectl get nodes"
    kubectl get nodes
    Write-Host ""

    # 2) Pulizia Docker
    # Write-Host "[RUN ] docker compose down -v (usa env temporaneo per evitare warning)"
    # $tmpEnv = Join-Path $env:TEMP "compose-down.env"
    # "Serilog__SerilogConfiguration__LoggerTelemetryOptions__ConnectionString=" | Out-File -FilePath $tmpEnv -Encoding ASCII -Force
    # if (Test-Path ".\docker-compose.yml") { docker compose --env-file "$tmpEnv" down -v 2>$null }
    # Remove-Item $tmpEnv -ErrorAction SilentlyContinue

    Write-Host "[RUN ] docker stop <tutti i container>"
    docker ps -aq | ForEach-Object { docker stop $_ } 2>$null
    Write-Host "[RUN ] docker rm <tutti i container>"
    docker ps -aq | ForEach-Object { docker rm $_ } 2>$null
    Write-Host "[RUN ] docker system prune -af"
    docker system prune -af
    Write-Host "[RUN ] docker builder prune -af"
    docker builder prune -af
    Write-Host "[RUN ] docker image prune -af"
    docker image prune -af
    Write-Host "[RUN ] docker volume prune -f"
    docker volume prune -f
    Write-Host "[RUN ] docker network prune -f"
    docker network prune -f
    Write-Host "[OK  ] Docker cleanup completato"
    Write-Host ""

    # 3) Build e push immagine
    Write-Host "[RUN ] docker login"
    docker login
    Write-Host "[RUN ] docker build -f Web.Api.Dockerfile -t $FULL ."
    docker build -f Web.Api.Dockerfile -t $FULL .
    Write-Host "[RUN ] docker push $FULL"
    docker push $FULL
    Write-Host "[OK  ] Build e push completati"
    Write-Host ""

    # 4) Pulizia Kubernetes
    Write-Host "[RUN ] kubectl delete -f loggerhelper-deployment.yaml --ignore-not-found"
    kubectl delete -f loggerhelper-deployment.yaml --ignore-not-found
    Write-Host "[RUN ] kubectl delete -f loggerhelper-service.yaml --ignore-not-found"
    kubectl delete -f loggerhelper-service.yaml --ignore-not-found
    Write-Host "[OK  ] Vecchie risorse rimosse"
    Write-Host ""

    # 5) Deploy nuova immagine
    Write-Host "[RUN ] kubectl apply -f loggerhelper-deployment.yaml"
    kubectl apply -f loggerhelper-deployment.yaml
    Write-Host "[RUN ] kubectl apply -f loggerhelper-service.yaml"
    kubectl apply -f loggerhelper-service.yaml
    Write-Host "[RUN ] kubectl set image deploy/loggerhelper-api-deployment loggerhelper-api=$FULL"
    kubectl set image deploy/loggerhelper-api-deployment loggerhelper-api=$FULL
    Write-Host "[RUN ] kubectl rollout status deploy/loggerhelper-api-deployment"
    kubectl rollout status deploy/loggerhelper-api-deployment
    Write-Host "[OK  ] Deploy aggiornato a $TAG"
    Write-Host ""

    # 6) Verifica
    Write-Host "[RUN ] kubectl get pods -o wide"
    kubectl get pods -o wide
    Write-Host "[RUN ] kubectl describe deploy loggerhelper-api-deployment | Select-String 'Image'"
    kubectl describe deploy loggerhelper-api-deployment | Select-String -Pattern 'Image'
    Write-Host ""

    # 7) Log applicativi
    Write-Host "[RUN ] kubectl logs -l app=loggerhelper -f | Select-String connectionstring,mssql"
    kubectl logs -l app=loggerhelper -f | Select-String -Pattern 'connectionstring','mssql'
    Write-Host ""

    Write-Host "[DONE] Script completato con successo"
}
catch {
    Write-Host ""
    Write-Host "[ERROR] Script failed" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
    Write-Host "[HINT ] Verifica: Docker attivo, login Docker Hub, contesto Kubernetes." -ForegroundColor Yellow
    exit 1
}
