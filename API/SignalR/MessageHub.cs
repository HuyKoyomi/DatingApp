using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR;

// quản lý kết nối thời gian thực giữa client và server
public class MessageHub(IMessageRepository messageRepository) : Hub
{

    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext(); // Lấy đối tượng HttpContext của kết nối hiện tại
        var otherUser = httpContext?.Request.Query["user"]; // Lấy giá trị tham số user từ query string của HTTP request (tức là tên người dùng đối tác mà người dùng hiện tại muốn trò chuyện).

        if (Context.User == null || string.IsNullOrEmpty(otherUser)) throw new Exception("Cannot join group"); // Kiểm tra xem Context.User có tồn tại hay không
        var groupName = GetGroupName(Context.User.GetUserName(), otherUser); // tạo một tên nhóm trò chuyện duy nhất giữa hai người dùng
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName); // Thêm client vào nhóm trò chuyện

        var messages = await messageRepository.GetMessageThread(Context.User.GetUserName(), otherUser!); // Lấy lịch sử tin nhắn giữa hai người
        await Clients.Group(groupName).SendAsync("ReceiveMessageThread", messages); // Gửi lịch sử tin nhắn về client
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }

    private string GetGroupName(string caller, string? other)
    {
        // phương thức đặt tên nhóm trò chuyện
        var stringCompare = string.CompareOrdinal(caller, other) < 0;
        return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
    }

}

