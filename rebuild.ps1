# ========================
# Build & Deploy Pipeline
# ========================

#REMEMBER
		#Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass
		#change version here and on deployment.yaml!
#REMEMBER


# Variabili
$USER = "alexbypa"
$IMAGE = "loggerhelper-api"
$TAG   = "1.0.22"
$FULL  = "${USER}/${IMAGE}:${TAG}"
$NS    = "webapi"

function Exec($cmd, desc) {
    try {
        Write-Host "`n>>> Eseguo: $cmd" -ForegroundColor Cyan
        if ($desc) { Write-Host "`n>>> $desc" -ForegroundColor gray }
        Invoke-Expression $cmd
        if ($LASTEXITCODE -ne 0) { throw "Errore in comando: $cmd" }
    }
    catch {
        Write-Host "!!! Errore: $_" -ForegroundColor Red
        exit 1
    }
}

# 0) Pulizia locale
Exec 'docker ps -aq | ForEach-Object { docker rm -f $_ }'
Exec 'docker volume ls -q | ForEach-Object { docker volume rm $_ }'
Exec 'docker network ls --filter "type=custom" -q | ForEach-Object { docker network rm $_ }'
Exec 'docker system prune -af --volumes'
Exec 'if (kubectl get ns $NS 2>$null) { kubectl delete ns $NS }'
Exec 'kubectl create ns $NS'

# 1) Build immagine
Exec "docker build --no-cache -t $FULL -f Web.Api.Dockerfile ."
Exec "docker tag $FULL ${USER}/${IMAGE}:latest"

# 2) Verifica contenuto immagine
Exec "docker run --rm --entrypoint /bin/sh $FULL -lc 'ls -lh /app; echo ---; cat /cse-assemblies.log'"

$risp = Read-Host "I Packages sono aggiornati? (S/N)"
if ($risp -notin @('S', 's')) {
    Write-Host "Aggiorna i pacchetti prima di continuare." -ForegroundColor Yellow
    exit 0
}

# 3) Login e push
Exec 'docker login'
Exec "docker push $FULL"

# 4) Deploy Kubernetes
Exec 'kubectl apply -f loggerhelper-deployment.yaml'
Exec 'kubectl apply -f loggerhelper-service.yaml'
Exec 'kubectl apply -f adminer.yaml'

# 5) Post-deploy check
Exec "kubectl -n $NS get all"
Exec "kubectl -n default rollout status deployment/loggerhelper-api-deployment"
Exec "kubectl -n default logs deploy/loggerhelper-api-deployment --tail=200"

# 6) Accesso locale
# Exec "kubectl -n $NS port-forward svc/loggerhelper-api 8080:8080"

# 7) Restart rapido
# Exec "kubectl -n $NS rollout restart deployment/loggerhelper-api"

# 8) Hard reset opzionale
# Exec "docker system prune -af --volumes"
# Exec "kubectl delete ns $NS"

# Crea DB (opzionale)
# Exec 'kubectl run mssql-tools --rm -it --restart=Never --image=mcr.microsoft.com/mssql-tools -- /opt/mssql-tools/bin/sqlcmd -S mssql,1433 -U sa -P "strong!password#123" -Q "CREATE DATABASE [test_db];"'

Write-Host "`nPipeline completata con successo." -ForegroundColor Green
