using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Configurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
     {
        builder.ToTable("UserRoles");

        builder.HasKey(ur => ur.Id);
        builder.Property(ur => ur.Id)
            .HasColumnType("integer")
            .UseIdentityAlwaysColumn();

        builder.Property(ur => ur.UserId)
            .HasColumnType("integer");
        builder.Property(ur => ur.RoleId)
            .HasColumnType("integer");

        builder.HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);

        builder.HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);
    }
}