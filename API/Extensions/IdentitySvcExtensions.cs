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

            // cấu hình xác thực bằng JWT (JSON Web Token) cho ứng dụng ASP.NET Core.
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

                // Xác thực Token từ Query String cho SignalR
                // SignalR sử dụng WebSockets, nhưng WebSockets không thể gửi HTTP Headers (và do đó không thể gửi Authorization: Bearer <token> trong request header như API REST thông thường). 
                // → Vì vậy, client cần gửi token qua query string (?access_token=<JWT>).
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"]; // Kiểm tra xem có access_token trong query string không

                        var path = context.HttpContext.Request.Path;
                        // Nếu có và request bắt đầu bằng /hubs (tức là kết nối đến SignalR Hub), thì gán token vào context.Token để hệ thống xác thực.
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
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