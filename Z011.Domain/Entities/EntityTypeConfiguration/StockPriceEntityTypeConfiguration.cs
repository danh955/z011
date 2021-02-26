// <copyright file="StockPriceEntityTypeConfiguration.cs" company="None">
// Free and open source code.
// </copyright>
namespace Z011.Domain.Entities.EntityTypeConfiguration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// Stock price entity type configuration class.
    /// </summary>
    internal class StockPriceEntityTypeConfiguration : IEntityTypeConfiguration<StockPriceEntity>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<StockPriceEntity> builder)
        {
            builder.ToTable("StockPrices", SchemaNames.Stocks);
            builder.HasKey(p => p.Id);
            builder.Property(p => p.StockId).HasColumnName("StockId").IsRequired();
            builder.Property(p => p.Frequency).HasColumnName("Frequency").IsRequired();
            builder.Property(p => p.Period).HasColumnName("Period").IsRequired();
            builder.Property(p => p.Low).HasColumnName("Low");
            builder.Property(p => p.High).HasColumnName("High");
            builder.Property(p => p.Close).HasColumnName("Close");
            builder.Property(p => p.AdjClose).HasColumnName("AdjClose");
            builder.Property(p => p.Volume).HasColumnName("Volume");

            builder.HasIndex(p => new { p.StockId, p.Frequency, p.Period }).IsUnique();

            // Stock has many stock prices.
            builder.HasOne<StockEntity>(p => p.Stock)
                .WithMany(s => s.StockPrices)
                .HasForeignKey(p => p.StockId);
        }
    }
}