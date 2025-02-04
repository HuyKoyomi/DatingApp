using API.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API;

public class LogUserActivity : IAsyncActionFilter
{
    // ghi lại thời điểm cuối cùng người dùng hoạt động trên hệ thống.
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var resultContext = await next(); // Lấy kết quả thực thi action

        if (context.HttpContext.User.Identity?.IsAuthenticated != true) return; // Kiểm tra xem người dùng có được xác thực không

        var username = resultContext.HttpContext.User.GetUserName();

        var repo = resultContext.HttpContext.RequestServices.GetRequiredService<IUserRepository>(); // Lấy repository và thông tin người dùng
        var user = await repo.GetUserByUsernameAsync(username);
        if (user == null) return;
        user.LastActive = DateTime.UtcNow;
        await repo.SaveAllAsync();
    }
}