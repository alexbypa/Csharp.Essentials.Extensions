1) Installare Microsoft.EntityFrameworkCore
2) La migration cercherà i DBContext registrati tramite DI
3) Quindi applicherà le modifiche sul DB cercando ogni DBSet
4) dotnet add package Microsoft.EntityFrameworkCore.SqlServer
4) dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
4) dotnet add Microsoft.Extensions.Configuration.Json


Sì, **le migration di EF Core usano AppDbContextFactory solo in un contesto specifico: la CLI.**
---

> ❓ **La migration viene eseguita da `AppDbContextFactory`?**
> ✅ **Sì, ma solo quando lanci comandi CLI come:**

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

EF cerca automaticamente una classe che implementa:

```csharp
IDesignTimeDbContextFactory<YourDbContext>
```


per creare un'istanza del tuo `DbContext` **fuori dal contesto dell'app**, come nel caso della migration.

---

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

## ⚠️ Se `AppDbContextFactory` non è in un progetto referenziato...

Allora la CLI **non può nemmeno trovarlo**. Soluzione:

1. Il progetto dove scrivi `AppDbContextFactory` deve essere **compilabile**
2. Il progetto dove esegui `dotnet ef ...` deve avere `Microsoft.EntityFrameworkCore.Design` e referenziare il progetto `DataAccessLayer`

---

## ✅ Verifica

Esegui da terminale nella WebAPI:

```bash
dotnet ef dbcontext list
```

Se ti mostra `ServiceDbContext`, allora `AppDbContextFactory` è **correttamente agganciato**.

---
Attenzione: 
Your startup project 'Web.Api' doesn't reference Microsoft.EntityFrameworkCore.Design. This package is required for the Entity Framework Core Tools to work. Ensure your startup project is correct, install the package, and try again.


Quindi, dalla root usare 
--1
dotnet ef dbcontext list --project DataAccessLayer\DataAccessLayer.csproj --startup-project Web.Api\Web.Api.csproj
--2
dotnet ef migrations add InitSchema --project DataAccessLayer\DataAccessLayer.csproj --startup-project Web.Api\Web.Api.csproj
--3
