// <copyright file="Program.cs" company="None">
// Free and open source code.
// </copyright>

namespace Z011.ConsoleApp
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Z011.Application.Extensions;
    using Z011.Application.StockPriceUpdater;
    using Z011.Domain.Entities;
    using Z011.Infrastructure.Extensions;

    /// <summary>
    /// Main program class.
    /// https://docs.microsoft.com/en-us/dotnet/core/extensions/generic-host.
    /// https://dfederm.com/building-a-console-app-with-.net-generic-host.
    /// </summary>
    internal static class Program
    {
        private const string SqliteConnecttionString = @"DataSource=C:\Code\DB\Test.SQLite3";

        /// <summary>
        /// Program starts here.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        /// <returns>Task.</returns>
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args)
                        .UseConsoleLifetime()
                        .Build();

            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            await EnsureDatabaseIsCreated(services);
            await MainAppAsync(services.GetRequiredService<IMediator>(), CancellationToken.None);
        }

        /// <summary>
        /// Create host builder.
        /// Note: Entity framework migrations uses this function.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        /// <returns>IHostBuilder.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    string name = typeof(Program).Namespace;
                    services.AddDbContextPool<EntityDbContext>(options => options.UseSqlite(SqliteConnecttionString, p => p.MigrationsAssembly(name)));
                    services.AddPooledDbContextFactory<EntityDbContext>(options => options.UseSqlite(SqliteConnecttionString, p => p.MigrationsAssembly(name)));

                    services.AddApplication();
                    services.AddInfrastructure();
                });

        private static async Task MainAppAsync(IMediator mediator, CancellationToken cancellationToken)
        {
            //// await mediator.Send(new StockSymbolUpdaterCommand(), cancellationToken);

            await mediator.Send(new StockPriceUpdaterCommand() { Frequency = Frequency.Monthly }, cancellationToken);

            ////var gridResult = await mediator.Send(new StockChangeGrid.Query(), cancellationToken);
            ////Console.WriteLine($"There are {gridResult.Rows.Count()} rows.");
        }

        private static async Task EnsureDatabaseIsCreated(IServiceProvider services)
        {
            var contextFactory = services.GetRequiredService<IDbContextFactory<EntityDbContext>>();
            using var context = contextFactory.CreateDbContext();
            //// await context.Database.EnsureCreatedAsync();
            await context.Database.MigrateAsync();
        }
    }
}