using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .HasColumnType("integer")
            .UseIdentityAlwaysColumn();

        builder.Property(a => a.ProductName)
            .HasColumnType("varchar");

        builder.Property(a => a.Date)
            .HasColumnType("timestamp");
        builder.Property(a => a.UserId)
            .HasColumnType("integer");

        builder.HasOne(a => a.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(a => a.UserId);
    }
}