// <copyright file="StockEntityTypeConfiguration.cs" company="None">
// Free and open source code.
// </copyright>
namespace Z011.Domain.Entities.EntityTypeConfiguration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// Stock entity type configuration class.
    /// </summary>
    internal class StockEntityTypeConfiguration : IEntityTypeConfiguration<StockEntity>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<StockEntity> builder)
        {
            builder.ToTable("Stocks", SchemaNames.Stocks);
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Symbol).HasColumnName("Symbol").IsRequired();
            builder.Property(s => s.Name).HasColumnName("Name");

            builder.HasIndex(s => s.Symbol).IsUnique();
        }
    }
}