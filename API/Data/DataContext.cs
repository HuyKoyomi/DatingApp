using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContext(DbContextOptions options) : IdentityDbContext<AppUser, AppRole, int,
IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>,
 IdentityUserToken<int>>(options)
{
    public DbSet<UserLike> Likes { get; set; }
    public DbSet<Message> Messages { get; set; }


    // tạo model từ các entity classes + cấu hình các ràng buộc, quan hệ giữa các bảng
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<AppUser>()
            .HasMany(ur => ur.UserRoles)
            .WithOne(u => u.User)
            .HasForeignKey(ur => ur.UserId)
            .IsRequired();

        builder.Entity<AppRole>()
            .HasMany(ur => ur.UserRoles)
            .WithOne(u => u.Role)
            .HasForeignKey(ur => ur.Role)
            .IsRequired();

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

        builder.Entity<Message>()
            .HasOne(s => s.Recipient) // 1 mess - 1 Recipient
            .WithMany(l => l.MessSent)
            .OnDelete(DeleteBehavior.Restrict); // nếu có mess nào tham chiếu đến Recipient => không được phép xóa

        builder.Entity<Message>()
            .HasOne(s => s.Sender)
            .WithMany(l => l.MessReceived)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
