using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<AppUser> Users { get; set; }
    public DbSet<Interest> Interest { get; set; }
    public DbSet<UserInterest> UserInterest { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Cấu hình bảng trung gian UserInterest
        modelBuilder.Entity<UserInterest>()
        .HasKey(ui => new { ui.AppUserId, ui.InterestId }); // Khóa chính là tổ hợp

        modelBuilder.Entity<UserInterest>()
            .HasOne(ui => ui.AppUser)
            .WithMany(u => u.UserInterests)
            .HasForeignKey(ui => ui.AppUserId);

        modelBuilder.Entity<UserInterest>()
            .HasOne(ui => ui.Interest)
            .WithMany(i => i.UserInterests)
            .HasForeignKey(ui => ui.InterestId);
    }
}
