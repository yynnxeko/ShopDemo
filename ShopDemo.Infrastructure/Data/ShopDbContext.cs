using Microsoft.EntityFrameworkCore;
using ShopDemo.Core.Entities;

namespace ShopDemo.Infrastructure.Data;

public class ShopDbContext : DbContext
{
    public ShopDbContext(DbContextOptions<ShopDbContext> options) : base(options)
    {
    }

    // DbSet
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();                   // THÊM DÒNG NÀY
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 1. Unique email
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // 2. User → Role (1-n)
        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);   // Không cho xóa Role nếu còn User dùng

        // 3. RefreshToken → User
        modelBuilder.Entity<RefreshToken>()
            .HasOne(rt => rt.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // 4. Tự động apply tất cả IEntityTypeConfiguration (nếu bạn dùng sau này)
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShopDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}