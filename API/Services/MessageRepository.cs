using API.Data;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class MessageRepository(DataContext context, IMapper mapper) : IMessageRepository
{
    public void AddGroup(Group group)
    {
        context.Groups.Add(group);
    }

    public void AddMessage(Message message)
    {
        context.Messages.Add(message);
    }

    public void DeleteMessage(Message message)
    {
        context.Messages.Remove(message);
    }

    public async Task<Connection?> GetConnection(string connectionId)
    {
        return await context.Connections.FindAsync(connectionId);
    }

    public async Task<Group?> GetGroupForConnection(string connectionId)
    {
        return await context.Groups
         .Include(x => x.Connections)
         .Where(x => x.Connections.Any(c => c.ConnectionId == connectionId))
         .FirstOrDefaultAsync();
    }

    public async Task<Message?> GetMessage(int id)
    {
        return await context.Messages.FindAsync(id);
    }

    public async Task<Group?> GetMessageGroup(string groupName)
    {
        return await context.Groups
        .Include(x => x.Connections)
        .FirstOrDefaultAsync(x => x.Name == groupName);
    }

    public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
    {
        var query = context.Messages
        .OrderByDescending(x => x.MessageSent)
        .AsQueryable();

        query = messageParams.Container switch
        {
            "Inbox" => query.Where(x => x.Recipient.UserName == messageParams.UserName && x.RecipientDeleted == false), // Lấy các tin nhắn đến
            "Outbox" => query.Where(x => x.Sender.UserName == messageParams.UserName && x.SenderDeleted == false), // Lấy các tin nhắn gửi
            _ => query.Where(x => x.Recipient.UserName == messageParams.UserName && x.DateRead == null && x.RecipientDeleted == false)
        };

        var messages = query.ProjectTo<MessageDto>(mapper.ConfigurationProvider);
        return await PagedList<MessageDto>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
    }

    public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUserName, string recipientUserName)
    {
        // Lấy ra danh sách các tin nhắn trao đổi giữa hai người dùng (người hiện tại và người nhận)
        var messages = await context.Messages
            .Where(x =>
                x.RecipientUserName == currentUserName && x.RecipientDeleted == false && x.SenderUserName == recipientUserName ||
                x.SenderUserName == currentUserName && x.SenderDeleted == false && x.RecipientUserName == recipientUserName
            )
            .OrderBy(x => x.MessageSent) // sắp xếp theo thứ tự thời gian gửi
            .ProjectTo<MessageDto>(mapper.ConfigurationProvider)
            .ToListAsync();

        // Đánh dấu các tin nhắn chưa đọc 
        var unreadMess = messages.Where(x => x.DateRead == null &&
          x.RecipientUserName == currentUserName
        ).ToList();

        // cập nhật tất cả các tin nhắn thành đã đọc
        if (unreadMess.Count != 0)
        {
            unreadMess.ForEach(x => x.DateRead = DateTime.UtcNow);
            await context.SaveChangesAsync();
        }

        // đổi dữ liệu sang DTO
        return messages;

    }

    public void RemoveConncetion(Connection connection)
    {
        context.Connections.Remove(connection);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }
}