// <copyright file="Program.cs" company="None">
// Free and open source code.
// </copyright>

namespace Z011.ConsoleApp
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Z011.Application;
    using Z011.Application.Extensions;
    using Z011.Application.StockSymbolUpdater;
    using Z011.Domain.Entities;
    using Z011.Infrastructure.Extensions;
    using Z011.Persistence.SQLite.Extensions;

    /// <summary>
    /// Main program class.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Program starts here.
        /// </summary>
        /// <returns>Task.</returns>
        internal static async Task Main()
        {
            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddApplication();
                    services.AddInfrastructure();
                    services.AddPersistence(@"DataSource=C:\Code\DB\Test.SQLite3");
                }).UseConsoleLifetime();

            var host = builder.Build();

            using var serviceScope = host.Services.CreateScope();
            var services = serviceScope.ServiceProvider;
            EnsureDatabaseIsCreated(services);
            await MainAppAsync(services.GetRequiredService<IMediator>());
        }

        private static async Task MainAppAsync(IMediator mediator)
        {
            await mediator.Send(new StockSymbolUpdaterCommand());

            var gridResult = await mediator.Send(new StockChangeGrid.Query());
            Console.WriteLine($"There are {gridResult.Rows.Count()} rows.");
        }

        private static void EnsureDatabaseIsCreated(IServiceProvider services)
        {
            var contextFactory = services.GetRequiredService<IDbContextFactory<EntityDbContext>>();
            using var context = contextFactory.CreateDbContext();
            context.Database.EnsureCreated();
        }
    }
}