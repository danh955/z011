// <copyright file="ServiceCollectionExtensions.cs" company="None">
// Free and open source code.
// </copyright>
namespace Z011.Persistence.SQLite.Extensions
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Z011.Domain.Entities;

    /// <summary>
    /// Service collection extensions class.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add persistence project to the service collection.
        /// </summary>
        /// <param name="service">IServiceCollection.</param>
        /// <param name="sqliteConnecttionString">SQLite connection string.</param>
        /// <returns>Updated IServiceCollection.</returns>
        public static IServiceCollection AddPersistence(this IServiceCollection service, string sqliteConnecttionString)
        {
            service.AddPooledDbContextFactory<EntityDbContext>(options => options.UseSqlite(sqliteConnecttionString));
            return service;
        }
    }
}