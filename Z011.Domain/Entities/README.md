# Entities

The database entities are managed in the Domain.Entities name space.

### Code first entity framework

- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core)
- [DbContext Lifetime, Configuration, and Initialization](https://docs.microsoft.com/en-us/ef/core/dbcontext-configuration)
- [Design-time DbContext Creation](https://docs.microsoft.com/en-us/ef/core/cli/dbcontext-creation)
 
### Migration

To create the migration for the first time, do the Add-Migration.

- [Microsoft.EntityFrameworkCore.Tools](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Tools)
- **Add-Migration** *name*
  - Creates a new migration class as per specified name with the Up() and Down() methods,
- **Update-Database**
  - Executes the last migration file created by the Add-Migration command and applies changes to the database schema.

### Add to your startup or main program

```csharp
.ConfigureServices((hostContext, services) =>
{
    string name = typeof(Program).Namespace;
    services.AddDbContextPool<EntityDbContext>(
                options => options.UseSqlite(
                                SqliteConnecttionString,
                                p => p.MigrationsAssembly(name)));
    services.AddPooledDbContextFactory<EntityDbContext>(
                options => options.UseSqlite(
                                SqliteConnecttionString,
                                p => p.MigrationsAssembly(name)));
}
```
The p.MigrationsAssembly(name) will put the migration in the project where this is declared.
The database entities will live in the Domain.Entities name space.
Replace "UseSqlite" with the relational database of your choice.

Note: We will find out if AddDbContextPool and AddPooledDbContextFactory can coexist with each other.
