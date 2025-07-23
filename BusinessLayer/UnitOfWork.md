## ✅ 1. **Verifica dei principi SOLID sulla tua implementazione `Unit of Work`**

| Principio                     | Stato | Note                                                                            |
| ----------------------------- | ----- | ------------------------------------------------------------------------------- |
| **S** – Single Responsibility | ✅     | `Repository<>` gestisce le entità, `UnitOfWork` coordina il salvataggio         |
| **O** – Open/Closed           | ✅     | Puoi estendere con `Repository<CustomEntity>` senza modificare codice esistente |
| **L** – Liskov Substitution   | ✅     | `IRepository<T>` può essere sostituito con finti/mocks                          |
| **I** – Interface Segregation | ✅     | Interfacce specifiche come `IGitHubOptionsRepo` evitano overload generici       |
| **D** – Dependency Inversion  | ✅     | `IUnitOfWork` e `IRepository<T>` sono iniettati, non istanziati                 |

🔧 **Bonus migliorabile**: potresti centralizzare la registrazione in un metodo `AddUnitOfWorkInfrastructure(IServiceCollection)` per chiarezza.

---

## 🧾 2. README.md – conciso, chiaro, con istruzioni client

````md
# 🔁 Unit of Work – CSharpEssentials.DataAccess

This package provides a lightweight, SOLID-compliant implementation of the **Unit of Work + Repository** pattern for Entity Framework Core.

---

## ✅ Architecture Overview

- `IRepository<TEntity>` – Generic repository for common CRUD operations
- `IUnitOfWork` – Abstracts transaction and repository access
- `Repository<TContext, TEntity>` – Generic repository implementation
- `EfUnitOfWork` – Default Unit of Work class
- `ServiceDbContext` – EF Core DbContext used by repositories

---

## 🧱 Base Classes (Do not change)

| File | Description |
|------|-------------|
| `IRepository.cs` | Generic abstraction over entity operations |
| `Repository<TContext, TEntity>.cs` | Base implementation using EF Core |
| `IUnitOfWork.cs` | Abstracts `SaveChangesAsync` and repository access |
| `EfUnitOfWork.cs` | Generic UoW implementation using DI |
| `ServiceDbContext.cs` | Your DbContext (EF) passed into UnitOfWork |

---

## 🚀 How to use in a Client Project

### 1. Register in `Program.cs`

```csharp
builder.Services.AddDbContext<ServiceDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default"))
);

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<ServiceDbContext, >));
builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork>();
builder.Services.AddScoped<IGitHubOptionsRepo, GitHubOptionsRepo>();
````

---

### 2. Create your Entity-specific Repository (optional)

```csharp
public interface IGitHubOptionsRepo
{
    Task<IEnumerable<GitHubOptions>> GetByUserName(string username);
}

public class GitHubOptionsRepo : IGitHubOptionsRepo
{
    private readonly IRepository<GitHubOptions> _repo;

    public GitHubOptionsRepo(IRepository<GitHubOptions> repo)
    {
        _repo = repo;
    }

    public Task<IEnumerable<GitHubOptions>> GetByUserName(string username)
    {
        return Task.FromResult(_repo.Query().Where(x => x.UserName == username).AsEnumerable());
    }
}
```

---

### 3. Use in Minimal API or Controller

```csharp
app.MapGet("/github-options", async (
    [FromServices] IGitHubOptionsRepo repo,
    [FromQuery] string username) =>
{
    var result = await repo.GetByUserName(username);
    return Results.Ok(result);
});
```

---

## ⚙️ Tip for Reusability

Wrap your registrations in a helper:

```csharp
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<ServiceDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("Default")));

        services.AddScoped(typeof(IRepository<>), typeof(Repository<ServiceDbContext, >));
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();
        return services;
    }
}
```

---

## 🧪 Unit Test Friendly

* `IRepository<T>` and `IUnitOfWork` are easy to mock
* Keep repository logic clean and test business logic independently

---

## 📦 Dependencies

* Microsoft.EntityFrameworkCore
* Microsoft.Extensions.DependencyInjection

---

## ✅ Status

✔ Fully SOLID
✔ Reusable
✔ Multi-provider ready (PostgreSQL, SQL Server)
✔ Minimal registration effort

```

---