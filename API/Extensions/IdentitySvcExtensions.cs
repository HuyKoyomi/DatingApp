using System.Text;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions
{
    public static class IdentitySvcExtensions
    {
        public static IServiceCollection AddIdentitySvc(this IServiceCollection services, IConfiguration config)
        {
            // AddIdentityCore<AppUser>: Đăng ký Identity nhưng chỉ với chức năng quản lý người dùng (không bao gồm các UI hoặc API mặc định).
            services.AddIdentityCore<AppUser>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
            })
                .AddRoles<AppRole>() // Thêm hỗ trợ quản lý vai trò (Role). => cho phép sử dụng hệ thống quyền dựa trên vai trò.
                .AddRoleManager<RoleManager<AppRole>>() // Đăng ký RoleManager: Đây là service dùng để quản lý vai trò (Role), bao gồm tạo, xóa, và cập nhật vai trò.
                .AddEntityFrameworkStores<DataContext>(); // Sử dụng DataContext làm nguồn dữ liệu cho Identity.

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                var tokenKey = config["TokenKey"] ?? throw new Exception("TokenKey not found");
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // Cấu hình Authorization => yêu cầu user phải có role cụ thể mới truy cập được
            services.AddAuthorizationBuilder()
                .AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"))
                .AddPolicy("ModeratePhotoRole", policy => policy.RequireRole("Admin", "Moderation"));
            return services;
        }
    }
}