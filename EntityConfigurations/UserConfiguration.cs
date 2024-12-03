using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
            .HasColumnType("integer")
            .UseIdentityAlwaysColumn();

        builder.Property(u => u.FirstName)
            .HasColumnType("varchar");
        builder.Property(u => u.LastName)
            .HasColumnType("varchar");
        builder.Property(u => u.AddressId)
            .HasColumnType("integer");

        builder.HasOne(u => u.Address)
            .WithMany()
            .HasForeignKey(u => u.AddressId);

        builder.HasMany(u => u.UserRoles)
            .WithOne(ur => ur.User)
            .HasForeignKey(ur => ur.UserId);

        builder.HasMany(u => u.Orders)
            .WithOne(o => o.User)
            .HasForeignKey(o => o.UserId);
    }
}