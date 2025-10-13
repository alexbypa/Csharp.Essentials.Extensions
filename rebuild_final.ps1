# ========================
# Build & Deploy Pipeline
# ========================

# Variabili
$USER = "alexbypa"
$IMAGE = "loggerhelper-api"
$TAG   = "1.0.21"
$FULL  = "${USER}/${IMAGE}:${TAG}"
$NS    = "default"
$DEPLOY = "loggerhelper-api-deployment"

function Exec($cmd, $desc) {
    try {
        if ($desc) { Write-Host "`n>>> $desc" -ForegroundColor Yellow }
        Write-Host ">>> Eseguo: $cmd" -ForegroundColor Cyan
        Invoke-Expression $cmd
        if ($LASTEXITCODE -ne 0) { throw "Errore in comando: $cmd" }
    }
    catch {
        Write-Host "!!! Errore: $_" -ForegroundColor Red
        exit 1
    }
}

# 0) Pulizia locale
Exec 'docker ps -aq | ForEach-Object { docker rm -f $_ }' "Rimozione di tutti i container Docker attivi o fermi"
Exec 'docker volume ls -q | ForEach-Object { docker volume rm $_ }' "Rimozione di tutti i volumi Docker"
Exec 'docker network ls --filter "type=custom" -q | ForEach-Object { docker network rm $_ }' "Rimozione delle reti Docker personalizzate"
Exec 'docker system prune -af --volumes' "Pulizia cache, immagini e volumi orfani"
Exec 'if (kubectl get ns $NS 2>$null) { kubectl delete ns $NS }' "Eliminazione del namespace Kubernetes esistente"
Exec 'kubectl create ns $NS' "Creazione di un nuovo namespace Kubernetes"

# 1) Build immagine
Exec "docker build --no-cache -t $FULL -f Web.Api.Dockerfile ." "Build dell'immagine Docker del progetto senza cache"
Exec "docker tag $FULL ${USER}/${IMAGE}:latest" "Aggiunta del tag 'latest' all'immagine Docker"

# 2) Verifica contenuto immagine
Exec "docker run --rm --entrypoint /bin/sh $FULL -lc 'ls -lh /app; echo ---; cat /cse-assemblies.log'" "Verifica del contenuto dell'immagine e visualizzazione log di build"

# Conferma aggiornamento pacchetti
$risp = Read-Host "I Packages sono aggiornati? (S/N)"
if ($risp -notin @('S', 's')) {
    Write-Host "Aggiorna i pacchetti prima di continuare." -ForegroundColor Yellow
    exit 0
}

# 3) Login e push
Exec 'docker login' "Autenticazione su Docker Hub"
Exec "docker push $FULL" "Esecuzione del push dell'immagine su Docker Hub"

# 4) Deploy Kubernetes
Exec "kubectl -n $NS apply -f loggerhelper-deployment.yaml" "Applicazione del file di deployment su Kubernetes"
Exec "kubectl -n $NS apply -f loggerhelper-service.yaml" "Applicazione del file di servizio su Kubernetes"
Exec "kubectl -n $NS apply -f adminer.yaml" "Applicazione del deployment di Adminer su Kubernetes"

# 5) Post-deploy check
Exec "kubectl -n $NS get all" "Visualizzazione delle risorse nel namespace"
Exec "kubectl -n $NS rollout status deployment/$DEPLOY" "Verifica dello stato di rollout del deployment"
Exec "kubectl -n $NS logs deploy/$DEPLOY --tail=200" "Visualizzazione delle ultime 200 righe di log del deployment"

# 6) Accesso locale (facoltativo)
# Exec "kubectl -n $NS port-forward svc/loggerhelper-api 8080:8080" "Apertura porta locale per accesso diretto al servizio"

# 7) Restart rapido (facoltativo)
# Exec "kubectl -n $NS rollout restart deployment/$DEPLOY" "Riavvio rapido dei pod senza ridistribuzione completa"

# 8) Hard reset opzionale
# Exec "docker system prune -af --volumes" "Pulizia completa del sistema Docker"
# Exec "kubectl delete ns $NS" "Eliminazione completa del namespace Kubernetes"

# Creazione DB (opzionale)
# Exec 'kubectl run mssql-tools --rm -it --restart=Never --image=mcr.microsoft.com/mssql-tools -- /opt/mssql-tools/bin/sqlcmd -S mssql,1433 -U sa -P "strong!password#123" -Q "CREATE DATABASE [test_db];"' "Creazione database SQL Server nel cluster"

Write-Host "`nPipeline completata con successo." -ForegroundColor Green
