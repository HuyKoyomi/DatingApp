using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR;

// quản lý kết nối thời gian thực giữa client và server
public class MessageHub(IMessageRepository messageRepository, IUserRepository userRepository, IMapper mapper, IHubContext<PresenceHub> presenceHub) : Hub
{

    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext(); // Lấy đối tượng HttpContext của kết nối hiện tại
        var otherUser = httpContext?.Request.Query["user"]; // Lấy giá trị tham số user từ query string của HTTP request (tức là tên người dùng đối tác mà người dùng hiện tại muốn trò chuyện).

        if (Context.User == null || string.IsNullOrEmpty(otherUser)) throw new Exception("Cannot join group"); // Kiểm tra xem Context.User có tồn tại hay không
        var groupName = GetGroupName(Context.User.GetUserName(), otherUser); // tạo một tên nhóm trò chuyện duy nhất giữa hai người dùng
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName); // Thêm client vào nhóm trò chuyện
        await AddToGroup(groupName);

        var messages = await messageRepository.GetMessageThread(Context.User.GetUserName(), otherUser!); // Lấy lịch sử tin nhắn giữa hai người
        await Clients.Group(groupName).SendAsync("ReceiveMessageThread", messages); // Gửi lịch sử tin nhắn về client
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await RemoveFromMessageGroup();
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

        var groupName = GetGroupName(sender.UserName, recipient.UserName);
        var group = await messageRepository.GetMessageGroup(groupName);
        if (group != null && group.Connections.Any(x => x.Username == recipient.UserName))
        {
            message.DateRead = DateTime.UtcNow;
        }
        else
        {
            var connections = await PresenceTracker.GetConnectionsForUser(recipient.UserName);
            if (connections != null && connections?.Count != null)
            {
                await presenceHub.Clients.Clients(connections).SendAsync("NewMessageReceived",
                    new { username = sender.UserName, knownAs = sender.KnownAs });
            }
        }

        messageRepository.AddMessage(message); // Gọi phương thức AddMessage() từ messageRepository để lưu tin nhắn vào database.

        if (await messageRepository.SaveAllAsync())
        {
            await Clients.Group(groupName).SendAsync("NewMessage", mapper.Map<MessageDto>(message)); // Gửi tin nhắn (message) đến tất cả thành viên trong nhóm
        }
    }

    // Phương thức này dùng để thêm một kết nối (connection) vào một nhóm (group).
    private async Task<bool> AddToGroup(string groupName)
    {
        var userName = Context.User?.GetUserName() ?? throw new Exception("Cannnot get userName");

        var group = await messageRepository.GetMessageGroup(groupName); // Truy vấn nhóm (group) từ cơ sở dữ liệu dựa trên groupName.
        var connection = new Connection { ConnectionId = Context.ConnectionId, Username = userName };

        if (group == null)
        {
            group = new Group { Name = groupName };
            messageRepository.AddGroup(group); // Nếu chưa có nhóm, sẽ tạo nhóm mới.
        }
        group.Connections.Add(connection); // Thêm kết nối vào nhóm
        return await messageRepository.SaveAllAsync();
    }

    // Phương thức này dùng để xóa một kết nối khỏi nhóm tin nhắn.
    private async Task RemoveFromMessageGroup()
    {
        var connection = await messageRepository.GetConnection(Context.ConnectionId);
        if (connection != null)
        {
            messageRepository.RemoveConncetion(connection); // Xóa kết nối nếu tồn tại
            await messageRepository.SaveAllAsync();
        }
    }

    private string GetGroupName(string caller, string? other)
    {
        // phương thức đặt tên nhóm trò chuyện
        var stringCompare = string.CompareOrdinal(caller, other) < 0;
        return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
    }

}

