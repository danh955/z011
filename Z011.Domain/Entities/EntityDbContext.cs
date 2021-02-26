// <copyright file="EntityDbContext.cs" company="None">
// Free and open source code.
// </copyright>

namespace Z011.Domain.Entities
{
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Entity database context interface.
    /// </summary>
    public class EntityDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityDbContext"/> class.
        /// </summary>
        /// <param name="options">DbContextOptions.</param>
        public EntityDbContext(DbContextOptions<EntityDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the stock database entity.
        /// </summary>
        public DbSet<StockEntity> Stocks { get; set; }

        /// <summary>
        /// Gets or sets the stock price database entity.
        /// </summary>
        public DbSet<StockPriceEntity> StockPrices { get; set; }

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EntityDbContext).Assembly);
        }
    }
}