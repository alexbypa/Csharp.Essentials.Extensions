**STEPS**
1) Installare Microsoft.EntityFrameworkCore
2) dotnet add package Microsoft.EntityFrameworkCore.SqlServer
3) dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
4) dotnet add Microsoft.Extensions.Configuration.Json

---
## 🔥 Cosa fa partire la migration
> **La migration viene eseguita da AppDbContextFactory**
Questo perchè EF cerca automaticamente una classe che implementa:
```csharp
IDesignTimeDbContextFactory<YourDbContext>
```

## 🔥 Ma: non viene mai “eseguita” a runtime!
Se non hai qualcosa tipo:
```csharp
dbContext.Database.Migrate();
```
all’avvio della tua Web API, allora **le migration non vengono applicate automaticamente.**

---

## ✅ Come applicarle a runtime (opzionale)

Nel `Program.cs`, puoi aggiungere:

```csharp
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ServiceDbContext>();
    db.Database.Migrate(); // <-- applica tutte le migration pendenti
}
```

---

## ✅ Verifica e Azione

Esegui da terminale dalla root della soluzione :

1) dotnet ef dbcontext list --project DataAccessLayer\DataAccessLayer.csproj --startup-project Web.Api\Web.Api.csproj 
2) dotnet ef migrations add InitSchema --project DataAccessLayer\DataAccessLayer.csproj --startup-project Web.Api\Web.Api.csproj
3) dotnet ef database update --project DataAccessLayer\DataAccessLayer.csproj --startup-project Web.Api\Web.Api.csproj

### Il primo comando restiuisce i DBContext trovati !