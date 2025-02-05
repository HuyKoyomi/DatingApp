using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<AppUser> Users { get; set; } // Khai báo các DbSet
    public DbSet<UserLike> Likes { get; set; }

    // tạo model từ các entity classes + cấu hình các ràng buộc, quan hệ giữa các bảng
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<UserLike>().HasKey(k => new { k.SourceUserId, k.TargetUserId }); // Cấu hình khóa chính cho bảng UserLike

        builder.Entity<UserLike>()
        .HasOne(s => s.SourceUser) // UserLike có một SourceUser
        .WithMany(l => l.LikedUsers) // AppUser có thể được thích bởi nhiều UserLike
        .HasForeignKey(s => s.SourceUserId) // SourceUserId là khóa ngoại (foreign key) liên kết đến AppUser.
        .OnDelete(DeleteBehavior.Cascade); // Khi một AppUser bị xóa, tất cả các UserLike liên quan đến SourceUserId cũng sẽ bị xóa theo (cascade delete)

        builder.Entity<UserLike>()
        .HasOne(s => s.TargetUser)
        .WithMany(l => l.LikedByUsers)
        .HasForeignKey(s => s.TargetUserId)
        .OnDelete(DeleteBehavior.Cascade);
    }
}
