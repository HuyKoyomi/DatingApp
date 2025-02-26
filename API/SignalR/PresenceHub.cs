using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR;

// PresenceHub là một Hub trong SignalR giúp theo dõi trạng thái trực tuyến (online/offline) của người dùng
// Mỗi khi một client kết nối hoặc ngắt kết nối, Hub sẽ gửi sự kiện đến những client khác thông báo về trạng thái của người dùng.
[Authorize]
public class PresenceHub : Hub
{
    // Xử lý khi client kết nối (OnConnectedAsync)
    public override async Task OnConnectedAsync()
    {
        await Clients.Others.SendAsync("UserIsOnline", Context.User.GetUserName());
        // Lấy thông tin user đang kết nối thông qua Context.User.GetUserName()
        // Gửi sự kiện "UserIsOnline" đến tất cả client khác (Clients.Others), báo rằng user này đã online.
    }

    // Xử lý khi client ngắt kết nối (OnDisconnectedAsync)
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        // Gửi sự kiện "UserIsOffline" đến tất cả client khác, báo rằng user này đã offline.
        await Clients.Others.SendAsync("UserIsOffline", Context.User?.GetUserName());

        // Gọi base.OnDisconnectedAsync(exception); để đảm bảo rằng SignalR xử lý đúng các hành động mặc định khi một user mất kết nối.
        await base.OnDisconnectedAsync(exception);
    }

}