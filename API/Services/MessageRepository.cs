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
    public void AddMessage(Message message)
    {
        context.Messages.Add(message);
    }

    public void DeleteMessage(Message message)
    {
        context.Messages.Remove(message);
    }

    public async Task<Message?> GetMessage(int id)
    {
        return await context.Messages.FindAsync(id);
    }

    public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
    {
        var query = context.Messages
        .OrderByDescending(x => x.MessageSent)
        .AsQueryable();

        query = messageParams.Container switch
        {
            "Inbox" => query.Where(x => x.Recipient.Username == messageParams.Username), // Lấy các tin nhắn đến
            "Outbox" => query.Where(x => x.Sender.Username == messageParams.Username), // Lấy các tin nhắn gửi
            _ => query.Where(x => x.Recipient.Username == messageParams.Username && x.DateRead == null)
        };

        var messages = query.ProjectTo<MessageDto>(mapper.ConfigurationProvider);
        return await PagedList<MessageDto>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
    }

    public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername, string recipientUsername)
    {
        // Lấy ra danh sách các tin nhắn trao đổi giữa hai người dùng (người hiện tại và người nhận)
        var messages = await context.Messages
            .Include(x => x.Sender).ThenInclude(x => x.Photos)
            .Include(x => x.Recipient).ThenInclude(x => x.Photos)
            .Where(x =>
                x.RecipientUsername == currentUsername && x.SenderUsername == recipientUsername ||
                x.SenderUsername == currentUsername && x.RecipientUsername == recipientUsername
            )
            .OrderBy(x => x.MessageSent) // sắp xếp theo thứ tự thời gian gửi
            .ToListAsync();

        // Đánh dấu các tin nhắn chưa đọc 
        var unreadMess = messages.Where(x => x.DateRead == null &&
          x.RecipientUsername == currentUsername
        ).ToList();

        // cập nhật tất cả các tin nhắn thành đã đọc
        if (unreadMess.Count != 0)
        {
            unreadMess.ForEach(x => x.DateRead = DateTime.UtcNow);
            await context.SaveChangesAsync();
        }

        // đổi dữ liệu sang DTO
        return mapper.Map<IEnumerable<MessageDto>>(messages);

    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }
}