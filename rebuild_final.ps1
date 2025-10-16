# ========================
# Build & Deploy Pipeline

#Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass -Force

#change version here and on deployment.yaml!

# ========================

# ========================
# Build & Deploy end-to-end (PowerShell)
# ========================

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

function Exec([string]$cmd, [string]$msg = "") {
    if ($msg) { Write-Host "`n==> $msg" -ForegroundColor Cyan }
    Write-Host "    $cmd" -ForegroundColor DarkGray
    try {
        $out = Invoke-Expression $cmd
        if ($out) { $out }
    } catch {
        Write-Host "ERRORE: $($_.Exception.Message)" -ForegroundColor Red
        throw
    }
}

# ------------------------
# Parametri
# ------------------------
$USER  = "alexbypa"
$IMAGE = "loggerhelper-api"
$TAG   = "1.0.22"
$FULL  = "$USER/$IMAGE" + ":" + "$TAG"
$NS          = "webapi"          # namespace unico
$PG_SECRET   = "postgres-secret"
$PG_SVC      = "postgres"
$PG_STS      = "postgres"
$PG_STORAGE  = "5Gi"
$PG_USER     = "postgres"
$PG_DB       = "test_db"
$PG_PASS     = "strong!password#123"  # cambia se serve

$PGADMIN_DEP = "pgadmin"
$PGADMIN_SVC = "pgadmin"
$PGADMIN_MAIL = "admin@local"
$PGADMIN_PWD  = "adminadmin"          # cambia se serve

$DEPLOY = "loggerhelper-api-deployment"
$SVC    = "loggerhelper-api"

# ------------------------
# Helper: current namespace
# ------------------------
Exec "if (-not (kubectl get ns $NS 2>`$null)) { kubectl create ns $NS }" "Garantisce il namespace '$NS'"
Exec "kubectl config set-context --current --namespace=$NS" "Imposta il namespace corrente"

# ------------------------
# Metrics Server (per kubectl top)
# ------------------------
Write-Host "`n--- Metrics Server ---" -ForegroundColor Yellow
try {
    Exec "kubectl apply -f https://github.com/kubernetes-sigs/metrics-server/releases/latest/download/components.yaml -n kube-system" "Installa/aggiorna metrics-server"
    $patch = @'
[
  {"op":"add","path":"/spec/template/spec/containers/0/args/-","value":"--kubelet-insecure-tls"},
  {"op":"add","path":"/spec/template/spec/containers/0/args/-","value":"--kubelet-preferred-address-types=InternalIP,ExternalIP,Hostname"}
]
'@
    Exec "kubectl -n kube-system patch deploy metrics-server --type=json --patch '$patch'" "Patch args metrics-server (Docker Desktop)"
    Exec "kubectl -n kube-system rollout status deploy/metrics-server --timeout=90s" "Attende readiness metrics-server"
} catch {
    Write-Host "Warning: metrics-server non pronto. Continuerò." -ForegroundColor DarkYellow
}

# ------------------------
# Secret PostgreSQL + pgAdmin
# ------------------------
Exec "kubectl -n $NS delete secret $PG_SECRET 2>`$null; kubectl -n $NS create secret generic $PG_SECRET --from-literal=postgres-password='$PG_PASS'" "Secret PostgreSQL (password)"
Exec "kubectl -n $NS delete secret ${PGADMIN_DEP}-secret 2>`$null; kubectl -n $NS create secret generic ${PGADMIN_DEP}-secret --from-literal=PGADMIN_DEFAULT_EMAIL='$PGADMIN_MAIL' --from-literal=PGADMIN_DEFAULT_PASSWORD='$PGADMIN_PWD'" "Secret pgAdmin"

# ------------------------
# PostgreSQL: Headless SVC + StatefulSet con PVC per pod
# ------------------------
@"
apiVersion: v1
kind: Service
metadata:
  name: $PG_SVC
  namespace: $NS
  labels: { app: $PG_SVC }
spec:
  clusterIP: None
  selector: { app: $PG_SVC }
  ports:
  - name: postgres
    port: 5432
---
apiVersion: apps/v1
kind: StatefulSet
metadata:
  name: $PG_STS
  namespace: $NS
spec:
  serviceName: $PG_SVC
  replicas: 1
  selector:
    matchLabels: { app: $PG_SVC }
  template:
    metadata:
      labels: { app: $PG_SVC }
    spec:
      containers:
      - name: postgres
        image: postgres:16
        ports:
        - containerPort: 5432
          name: postgres
        env:
        - name: POSTGRES_USER
          value: "$PG_USER"
        - name: POSTGRES_DB
          value: "$PG_DB"
        - name: POSTGRES_PASSWORD
          valueFrom:
            secretKeyRef:
              name: $PG_SECRET
              key: postgres-password
        volumeMounts:
        - name: data
          mountPath: /var/lib/postgresql/data
  volumeClaimTemplates:
  - metadata:
      name: data
    spec:
      accessModes: ["ReadWriteOnce"]
      resources:
        requests:
          storage: $PG_STORAGE
"@ | kubectl apply -f - | Out-Null
Write-Host "PostgreSQL applicato." -ForegroundColor Green
Exec "kubectl -n $NS rollout status sts/$PG_STS --timeout=180s" "Attende readiness PostgreSQL"

# ------------------------
# pgAdmin: SVC + Deployment
# ------------------------
@"
apiVersion: v1
kind: Service
metadata:
  name: $PGADMIN_SVC
  namespace: $NS
  labels: { app: $PGADMIN_DEP }
spec:
  type: ClusterIP
  selector: { app: $PGADMIN_DEP }
  ports:
  - name: http
    port: 8080
    targetPort: 80
---
apiVersion: apps/v1
kind: Deployment
metadata:
  name: $PGADMIN_DEP
  namespace: $NS
spec:
  replicas: 1
  selector:
    matchLabels: { app: $PGADMIN_DEP }
  template:
    metadata:
      labels: { app: $PGADMIN_DEP }
    spec:
      containers:
      - name: pgadmin
        image: dpage/pgadmin4:8
        ports:
        - containerPort: 80
        env:
        - name: PGADMIN_DEFAULT_EMAIL
          valueFrom:
            secretKeyRef:
              name: ${PGADMIN_DEP}-secret
              key: PGADMIN_DEFAULT_EMAIL
        - name: PGADMIN_DEFAULT_PASSWORD
          valueFrom:
            secretKeyRef:
              name: ${PGADMIN_DEP}-secret
              key: PGADMIN_DEFAULT_PASSWORD
"@ | kubectl apply -f - | Out-Null
Write-Host "pgAdmin applicato." -ForegroundColor Green
Exec "kubectl -n $NS rollout status deploy/$PGADMIN_DEP --timeout=120s" "Attende readiness pgAdmin"

# ------------------------
# Build + Push immagine LoggerHelper API
# ------------------------
Exec "docker build --pull --no-cache -t $FULL -f Web.Api.Dockerfile ." "Build immagine $FULL"
Exec "docker tag $FULL $USER/$IMAGE:latest" "Tag latest"
Exec "docker push $FULL" "Push immagine versionata"
Exec "docker push $USER/$IMAGE:latest" "Push latest"

# ------------------------
# Deploy LoggerHelper API (Deployment + Service)
# - Nota: aggiorna ENV secondo le tue chiavi
# ------------------------
@"
apiVersion: v1
kind: Service
metadata:
  name: $SVC
  namespace: $NS
spec:
  selector: { app: $DEPLOY }
  ports:
  - port: 8081
    targetPort: 8081
    name: http
---
apiVersion: apps/v1
kind: Deployment
metadata:
  name: $DEPLOY
  namespace: $NS
spec:
  replicas: 1
  selector:
    matchLabels: { app: $DEPLOY }
  template:
    metadata:
      labels: { app: $DEPLOY }
    spec:
      containers:
      - name: api
        image: $FULL
        ports:
        - containerPort: 8081
        env:
        - name: Serilog__SerilogConfiguration__SerilogOption__PostgreSql__connectionString
          value: "Host=$PG_SVC.$NS.svc.cluster.local;Port=5432;Database=$PG_DB;Username=$PG_USER;Password=$PG_PASS"
        - name: ASPNETCORE_URLS
          value: "http://0.0.0.0:8081"
"@ | kubectl apply -f - | Out-Null
Exec "kubectl -n $NS rollout status deploy/$DEPLOY --timeout=180s" "Attende readiness LoggerHelper API"

# ------------------------
# Verifiche rapide
# ------------------------
Write-Host "`n--- Verifiche ---" -ForegroundColor Yellow
try { Exec "kubectl top nodes" "Uso risorse nodi" } catch { Write-Host "Metrics non disponibili." -ForegroundColor DarkYellow }
try { Exec "kubectl top pods -n $NS" "Uso risorse pod in $NS" } catch { }

Exec "kubectl -n $NS get pods -o wide" "Pods"
Exec "kubectl -n $NS get svc" "Services"
Exec "kubectl -n $NS get endpoints $PG_SVC" "Endpoint PostgreSQL"
Exec "kubectl -n $NS get endpoints $PGADMIN_SVC" "Endpoint pgAdmin"

# ------------------------
# Port-forward utili (facoltativi)
# ------------------------
Write-Host "`nPuoi aprire terminali separati per i port-forward:" -ForegroundColor DarkGray
Write-Host "  kubectl -n $NS port-forward svc/$PGADMIN_SVC 8080:8080   # pgAdmin" -ForegroundColor DarkGray
Write-Host "  kubectl -n $NS port-forward svc/$SVC       8081:8081   # LoggerHelper API" -ForegroundColor DarkGray

Write-Host "`nCompletato." -ForegroundColor Green
