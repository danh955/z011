// <copyright file="InMemoryDbContextFactorySqlite.cs" company="None">
// Free and open source code.
// </copyright>
namespace Z011.TestHelper
{
    using System;
    using System.Data.Common;
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;
    using Z011.Domain.Entities;

    /// <summary>
    /// In memory database context factory with SQLite class.
    /// From: https://www.meziantou.net/testing-ef-core-in-memory-using-sqlite.htm.
    /// </summary>
    public class InMemoryDbContextFactorySQLite : IDbContextFactory<EntityDbContext>, IDisposable
    {
        private DbConnection connection;

        /// <inheritdoc/>
        public EntityDbContext CreateDbContext()
        {
            if (this.connection == null)
            {
                this.connection = new SqliteConnection("DataSource=:memory:");
                this.connection.Open();
                this.EnsureCreated();
            }

            return new EntityDbContext(this.CreateOptions());
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (this.connection != null)
            {
                this.connection.Dispose();
                this.connection = null;
                GC.SuppressFinalize(this);
            }
        }

        private DbContextOptions<EntityDbContext> CreateOptions()
        {
            return new DbContextOptionsBuilder<EntityDbContext>()
                .UseSqlite(this.connection).Options;
        }

        private void EnsureCreated()
        {
            using var context = new EntityDbContext(this.CreateOptions());
            context.Database.EnsureCreated();
        }
    }
}