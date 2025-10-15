$pvc = @"
apiVersion: v1
kind: PersistentVolumeClaim
metadata:
  name: postgres-pvc
  namespace: webapi
spec:
  accessModes: [ "ReadWriteOnce" ]
  resources:
    requests:
      storage: 5Gi
"@
$pvc | kubectl apply -f -