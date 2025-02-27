using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR;

// quản lý kết nối thời gian thực giữa client và server
public class MessageHub(IMessageRepository messageRepository, IUserRepository userRepository, IMapper mapper) : Hub
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

    public async Task SendMessage(CreateMessageDto createMessageDto)
    {
        // Lấy username của người gửi
        var username = Context.User?.GetUserName() ?? throw new HubException("Could not get user");

        // Kiểm tra xem người dùng có gửi tin nhắn cho chính mình không
        if (username == createMessageDto.RecipientUserName.ToLower())
        {
            throw new HubException("You cannot send messages to yourself");
        }

        var sender = await userRepository.GetUserByUserNameAsync(username);
        var recipient = await userRepository.GetUserByUserNameAsync(createMessageDto.RecipientUserName);

        if (sender == null || recipient == null || sender.UserName == null || recipient.UserName == null) throw new HubException("Cannot send message at this time");

        var message = new Message
        {
            Sender = sender,
            Recipient = recipient,
            SenderUserName = sender.UserName,
            RecipientUserName = recipient.UserName,
            Content = createMessageDto.Content,
        };

        messageRepository.AddMessage(message); // Gọi phương thức AddMessage() từ messageRepository để lưu tin nhắn vào database.

        if (await messageRepository.SaveAllAsync())
        {
            var group = GetGroupName(sender.UserName, recipient.UserName); // Tạo tên nhóm trò chuyện giữa sender và recipient.
            await Clients.Group(group).SendAsync("NewMessage", mapper.Map<MessageDto>(message)); // Gửi tin nhắn (message) đến tất cả thành viên trong nhóm
        }
    }

    private string GetGroupName(string caller, string? other)
    {
        // phương thức đặt tên nhóm trò chuyện
        var stringCompare = string.CompareOrdinal(caller, other) < 0;
        return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
    }

}

