# Build pulito → Push Docker Hub → Deploy Kubernetes

## Variabili

```powershell
$USER = "alexbypa"                 # nome utente Docker Hub
$IMAGE = "loggerhelper-api"        # nome dell'immagine
$TAG   = "1.0.20"                  # tag versione
$FULL  = "$USER/$IMAGE:$TAG"       # ref completo immagine
$NS    = "webapi"                  # namespace Kubernetes
```

## 0) Azzerare locale

```powershell
# Rimuove tutti i container (running e stopped)
docker ps -aq | ForEach-Object { docker rm -f $_ }

# Rimuove tutti i volumi
docker volume ls -q | ForEach-Object { docker volume rm $_ }

# Rimuove tutte le reti custom (lascia bridge/host/none)
docker network ls --filter "type=custom" -q | ForEach-Object { docker network rm $_ }

# Pulisce immagini dangling, cache e volumi orfani
docker system prune -af --volumes
```

```powershell
# Se esiste, elimina il namespace e tutto il suo contenuto
if (kubectl get ns $NS 2>$null) { kubectl delete ns $NS }

# Ricrea il namespace vuoto
kubectl create ns $NS
```

## 1) Build immagine

```powershell
# Build senza cache usando il Dockerfile del progetto
docker build --no-cache -t $FULL -f Web.Api.Dockerfile .

# Aggiunge anche il tag :latest alla stessa immagine
docker tag $FULL "$USER/$IMAGE:latest"
```

## 2) Verifica contenuto immagine

```powershell
# Elenca DLL pubblicate e mostra il log versioni CSharpEssentials* generato in build
docker run --rm --entrypoint /bin/sh $FULL -lc "ls -lh /app; echo '---'; cat /cse-assemblies.log"
```

## 3) Login e push

```powershell
# Autenticazione a Docker Hub
docker login

# Push del tag versione
# crea i tag remoti
docker tag web-api:1.0.20 alexbypa/loggerhelper-api:1.0.20
docker tag web-api:1.0.20 alexbypa/loggerhelper-api:latest

# push
docker push alexbypa/loggerhelper-api:1.0.20
docker push alexbypa/loggerhelper-api:latest
```

## 4) Deploy su Kubernetes

```powershell
# Applica deployment e service dal repo
kubectl apply -f loggerhelper-deployment.yaml
kubectl apply -f loggerhelper-service.yaml
```

## 5) Controlli post-deploy

```powershell
# Vista sintetica delle risorse nel namespace
kubectl -n $NS get all

# Attende il completamento del rollout
kubectl rollout status deployment/loggerhelper-api

# Ultime 200 righe di log del pod del deployment
kubectl -n $NS logs deploy/loggerhelper-api --tail=200
```

## 6) Accesso locale senza Ingress

```powershell
# Espone in locale la porta 8080 del Service
kubectl -n $NS port-forward svc/loggerhelper-api 8080:8080
```

## 7) Restart rapido

```powershell
# Riavvia i pod del deployment senza ridistribuire i manifest
kubectl -n $NS rollout restart deployment/loggerhelper-api
```

## 8) Hard reset opzionale

```powershell
# Pulisce tutto Docker (attenzione: distruttivo)
docker system prune -af --volumes

# Elimina completamente il namespace (attenzione: distruttivo)
kubectl delete ns $NS
```
#Crea DB 
kubectl run mssql-tools --rm -it --restart=Never --image=mcr.microsoft.com/mssql-tools -- /opt/mssql-tools/bin/sqlcmd -S mssql,1433 -U sa -P "strong!password#123" -Q "CREATE DATABASE [test_db];"


kubectl get ns
kubectl get deploy -A | findstr loggerhelper
Se i pod sono in default:

powershell
Copia codice
kubectl -n default rollout restart deployment/loggerhelper-api-deployment
kubectl -n default rollout status deployment/loggerhelper-api-deployment
kubectl -n default logs deploy/loggerhelper-api-deployment --tail=200
Se vuoi usare webapi, prima crealo e sposta il deploy lì (oppure applica i manifest in webapi):

powershell
Copia codice
kubectl create ns webapi
kubectl -n webapi apply -f loggerhelper-deployment.yaml
kubectl -n webapi apply -f loggerhelper-service.yaml