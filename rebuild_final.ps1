param(
    [ValidateSet('install','verify')] [string]$Mode = 'install',
    [string]$Ns = 'webapi',
    [int]$TimeoutSec = 180,
    [string]$ServicesDir = 'k8s_services'
)

# --- guards ---
$ErrorActionPreference = 'Stop'
$ConfirmPreference = 'None'
$ProgressPreference = 'SilentlyContinue'
function Warn($m){ Write-Host $m -ForegroundColor Yellow }
function Info($m){ Write-Host $m -ForegroundColor Cyan }
function Debug($m){ Write-Host $m -ForegroundColor Gray }

# base paths
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition
Set-Location $ScriptDir
$SvcBase = Join-Path $ScriptDir $ServicesDir

function Wait-K8sApi([int]$T = 120){
    $sw=[Diagnostics.Stopwatch]::StartNew()
    while($sw.Elapsed.TotalSeconds -lt $T){
        try{
            kubectl cluster-info 1>$null 2>$null
            kubectl get ns default --request-timeout=5s 1>$null 2>$null
            return
        } catch { Start-Sleep 3 }
    }
    throw "API Kubernetes non raggiungibile dopo $T s."
}

function Ensure-Namespace([string]$ns){
		Debug('Ensure-Namespace: ' + $ns)
    if(-not (kubectl get ns $ns --ignore-not-found -o name)){ kubectl create ns $ns | Out-Null }
}

function Get-Workload([string]$name,[string]$ns,[string]$label=$null){
    Debug('Get-Workload: ' + $name)
    
    # Funzione Helper per ottenere l'oggetto JSON pulito.
    # Usiamo 'ignore-not-found' e reindirizziamo tutti i messaggi di errore (2)
    function Invoke-KubectlJson($resource, $query) {
			Debug("Eseguo Invoke-KubectlJson (" + $resource + "," + $query + ")")
			
			# FIX: Usa la stringa completa per garantire la corretta citazione degli argomenti
			$commandLine = "kubectl -n $ns get $resource $query --ignore-not-found -o json"
			
			# DEBUG: La tua stampa del comando completo
			Debug("Comando completo: " + $commandLine)

			# Esecuzione robusta con IEX (Invoke-Expression) per catturare l'output come singola stringa
			# Questo è il metodo più aggressivo per catturare l'output di un eseguibile
			$json = Invoke-Expression -Command "$commandLine 2>$null" | Out-String
			
			Debug("json (parziale): " + ($json.Substring(0, [Math]::Min(100, $json.Length))))
			Debug("----------------")
		
			if ([string]::IsNullOrWhiteSpace($json)) { return $null }
			try {
					return $json | ConvertFrom-Json
			} catch {
					Debug("WARNING: Parsing JSON fallito per $resource/$query. Output corrotto. Ritorno null.")
					return $null
			}
    }

    # 1. Ricerca per Nome (se $name è valorizzato)
    if ($name) {
        $dObj = Invoke-KubectlJson 'deploy' $name
        if($dObj){ return @{kind='Deployment';obj=$dObj} }
        
        $sObj = Invoke-KubectlJson 'statefulset' $name
        if($sObj){ return @{kind='StatefulSet';obj=$sObj} }
    }
    
    # 2. Ricerca per Label (se $label è valorizzato)
    if ($label) {
        $dlist = Invoke-KubectlJson 'deploy' "-l $label"
        if ($dlist -and $dlist.items -and $dlist.items.Count -gt 0){
            return @{kind='Deployment';obj=$dlist.items[0]}
        }
        
        $slist = Invoke-KubectlJson 'statefulset' "-l $label"
        if ($slist -and $slist.items -and $slist.items.Count -gt 0){
            return @{kind='StatefulSet';obj=$slist.items[0]}
        }
    }

    return $null
}


function Test-WorkloadReady([string]$name,[string]$ns,[string]$label=$null){
		Debug('Test-WorkloadReady: ' + $name)
    $w=Get-Workload $name $ns $label
    if(-not $w){ return $false }
    $obj=$w.obj
    $ready=($obj.status.readyReplicas|%{$_}); if(-not $ready){$ready=0}
    $desired=($obj.spec.replicas|%{$_});     if(-not $desired){$desired=0}
    return (($ready -ge 1) -and ($ready -eq $desired))
}

function Test-SvcHasEndpoints([string]$svc,[string]$ns){
	  Debug('Test-SvcHasEndpoints: ' + $svc)
    $e=kubectl -n $ns get endpoints $svc --ignore-not-found -o json 2>$null|ConvertFrom-Json
    if(-not $e){ return $false }
    $subs=@(); if($e -and $e.subsets){ $subs=$e.subsets }
    foreach($s in $subs){ if($s.addresses -and $s.ports){ return $true } }
    return $false
}

function Wait-WorkloadRollout([string]$label,[string]$ns,[int]$t){
    # Timeout di esistenza: usa $t (180s) con un minimo di 60s
    $timeoutExist = [Math]::Max($t, 60)
    $workloadName = $null
    $workloadKind = $null
    $sw = [Diagnostics.Stopwatch]::StartNew()

    Write-Host "Rollout: Attendo Workload con label '$label' nel ns '$ns'. Timeout max di esistenza: $($timeoutExist)s..." -ForegroundColor DarkGray
    
    # Ciclo di attesa per la CREAZIONE del Workload
    while ($sw.Elapsed.TotalSeconds -lt $timeoutExist) {
        # Riutilizza la funzione Get-Workload (che ora è più robusta)
        $w = Get-Workload $null $ns $label

        if ($w) {
            $workloadName = $w.obj.metadata.name
            $workloadKind = $w.kind
            break
        }
        
        Start-Sleep -Seconds 3 # Pausa di 3 secondi
    }

    # Verifica se il Workload è stato trovato
    if (-not $workloadName) {
        throw "Nessun workload con label '$label' nel ns '$ns' è apparso dopo $($timeoutExist)s. Controlla i logs: kubectl logs -n $ns -l $label"
    }

    # Attesa per il ROLLOUT (usando il nome trovato)
    Write-Host "Rollout: Trovato $($workloadKind)/$workloadName. Attesa completamento ($($t)s max)..." -ForegroundColor DarkGray
    
    # Adatta il tipo di risorsa per il comando rollout status
    $resourceType = if ($workloadKind -eq 'Deployment') { 'deploy' } else { 'statefulset' }
    
    kubectl -n $ns rollout status "$($resourceType)/$workloadName" --timeout=${t}s
    return
}

function Wait-ServiceEndpoints([string[]]$svcCandidates,[string]$ns,[int]$t=120){
		Debug('Wait-ServiceEndpoints: ' + $svcCandidates -join ", ")
    $sw=[Diagnostics.Stopwatch]::StartNew()
    while($sw.Elapsed.TotalSeconds -lt $t){
        foreach($svc in $svcCandidates){
            if(kubectl -n $ns get svc $svc --ignore-not-found -o name 2>$null){
                if(Test-SvcHasEndpoints $svc $ns){ return }
            }
        }
        Start-Sleep 2
    }
    throw ("Service senza endpoints dopo {0}s. Provati: {1}" -f $t, ($svcCandidates -join ', '))
}

function First-ExistingService([string[]]$candidates,[string]$ns){
		Debug('First-ExistingService: ' + $candidates -join ", ")
    foreach($c in $candidates){ if(kubectl -n $ns get svc $c --ignore-not-found -o name 2>$null){ return $c } }
    return $null
}

function Verify-Stack([string]$ns){
		Debug('Verify-Stack: ' + $ns)
    $checks=@(
        @{name='mssql';      svc=@('mssql');                        workload=''; label='app=mssql'},
        @{name='postgresql'; svc=@('postgres','postgres-container');workload=''; label='app=postgres'},
        @{name='pgadmin';    svc=@('pgadmin');                      workload=''; label='app=pgadmin'},
        @{name='adminer';    svc=@('adminer');                      workload=''; label='app=adminer'}
    )
    $missing=@(); $notReady=@(); $noEndpoints=@()
    foreach($c in $checks){
        $w=Get-Workload $c.workload $ns $c.label
        $svcName=First-ExistingService $c.svc $ns
        $workOK=Test-WorkloadReady $c.workload $ns $c.label
        $svcOK=$false; if($svcName){ $svcOK=Test-SvcHasEndpoints $svcName $ns }

        $statusWork = if($workOK){'OK'} else { if($w){'NOT-READY'} else {'MISSING'} }
        $statusSvc  = if($svcName){$svcName}else{'N/D'}
        $statusEndp = if($svcOK){'OK'} else { if($svcName){'NO-ENDPOINTS'} else {'N/A'} }

        "{0,-12} ns={1} workload={2} svc={3} endpoints={4}" -f $c.name,$ns,$statusWork,$statusSvc,$statusEndp

        if(-not $w){ $missing += $c.name } elseif(-not $workOK){ $notReady += $c.name }
        if($svcName -and -not $svcOK){ $noEndpoints += $c.name }
    }
    if($missing.Count -gt 0){ Warn ("Servizi non installati: " + ($missing -join ', ')) }
    if($notReady.Count -gt 0){ Warn ("Workload non Ready: " + ($notReady -join ', ')) }
    if($noEndpoints.Count -gt 0){ Warn ("Service senza endpoints: " + ($noEndpoints -join ', ')) }

    if($missing.Count -eq 0 -and $notReady.Count -eq 0 -and $noEndpoints.Count -eq 0){ "Stack OK nel namespace '$ns'."; exit 0 } else { exit 1 }
}

# Applica solo i file esistenti. Ritorna quanti sono stati applicati.
function Apply-Files([string[]]$files,[string]$ns){
		Debug('Apply-Files: ' + $files -join ", ")
    $applied=0
    foreach($f in $files){
        $full=Join-Path $SvcBase $f
        if(Test-Path $full){
            kubectl -n $ns apply -f $full | Out-Null
            $applied++
        } else {
            Info ("SKIP file mancante: {0}" -f $full)
        }
    }
    return $applied
}

function Ensure-Component([string]$name,[string[]]$files,[string]$workload,[string]$label,[string[]]$svcCandidates,[string]$ns){
		Debug('Ensure-Component: ' + $files -join ", ")

		$existsWork = [bool](Get-Workload $workload $ns $label)
    $existsSvc  = [bool](First-ExistingService $svcCandidates $ns)

		Debug('$existsWork: ' + $existsWork)
		Debug('$existsSvc: ' + $existsSvc)

    if($existsWork -and $existsSvc){
        Info ("SKIP install {0} (già presente in ns={1})" -f $name,$ns)
        return
    }

    $applied = Apply-Files -files $files -ns $ns
    if($applied -eq 0){
        Info ("SKIP install {0} (nessun manifest trovato in {1})" -f $name,$SvcBase)
        return
    }

    if($workload -or $label){ Wait-WorkloadRollout $label $ns $TimeoutSec }
    if($svcCandidates -and $svcCandidates.Count -gt 0){ Wait-ServiceEndpoints $svcCandidates $ns -t ([Math]::Min($TimeoutSec,120)) }
}

# --- start ---
if(-not (kubectl config current-context 2>$null)){ Write-Error "kubectl non configurato. Esegui: kubectl config use-context docker-desktop"; exit 1 }
Wait-K8sApi -T $TimeoutSec
Ensure-Namespace $Ns

if($Mode -eq 'verify'){
    Verify-Stack $Ns
    exit 0
}

# --- INSTALL ---  (file sotto .\k8s_services\)

Ensure-Component -name 'mssql'      -files @('mssql-deployment.yaml','mssql-service.yaml') `
                 -workload '' -label 'app=mssql'      -svcCandidates @('mssql')                     -ns $Ns

Ensure-Component -name 'postgresql' -files @('postgres-pvc.yaml','postgres.yaml','postgres-service.yaml') `
                 -workload '' -label 'app=postgres'   -svcCandidates @('postgres','postgres-container') -ns $Ns

Ensure-Component -name 'pgadmin'    -files @('pgadmin-pvc.yaml','pgadmin.yaml') `
                 -workload '' -label 'app=pgadmin'    -svcCandidates @('pgadmin')                  -ns $Ns

Ensure-Component -name 'adminer'    -files @('adminer.yaml') `
                 -workload '' -label 'app=adminer'    -svcCandidates @('adminer')                  -ns $Ns

# Web.API (opzionale)
# Ensure-Component -name 'webapi' -files @('webapi.yaml') `
#                  -workload '' -label 'app=loggerhelper-api' -svcCandidates @('loggerhelper-api') -ns $Ns

# --- VERIFICA FINALE ---
Verify-Stack $Ns