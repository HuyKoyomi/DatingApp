namespace API.SignalR;

public class PresenceTracker
{
    private static readonly Dictionary<string, List<string>> OnlineUsers = []; // lưu danh sách các kết nối đang hoạt động

    // Xử lý khi một user kết nối
    public Task UserConnected(string username, string connectionId)
    {
        lock (OnlineUsers) // Dùng lock (OnlineUsers) để đảm bảo luồng (thread-safe), tránh xung đột khi nhiều user kết nối cùng lúc.   
        {
            if (OnlineUsers.ContainsKey(username))
            {
                OnlineUsers[username].Add(connectionId); // Thêm connectionId vào danh sách
            }
            else
            {
                OnlineUsers.Add(username, [connectionId]); // Tạo một danh sách mới và thêm connectionId
            }
        }
        return Task.CompletedTask;
    }


    // Xử lý khi một user ngắt kết nối
    public Task UserDisconnected(string username, string connectionId)
    {
        lock (OnlineUsers)
        {
            if (!OnlineUsers.ContainsKey(username)) return Task.CompletedTask;

            OnlineUsers[username].Remove(connectionId); // Xoá connectionId của user

            if (OnlineUsers[username].Count == 0)
            {
                OnlineUsers.Remove(username); // Nếu user không còn kết nối nào, xoá khỏi OnlineUsers
            }
        }
        return Task.CompletedTask;
    }

    public Task<string[]> GetOnlineUsers()
    {
        string[] onlineUsers;
        lock (OnlineUsers)
        {
            onlineUsers = OnlineUsers.OrderBy(k => k.Key).Select(k => k.Key).ToArray();
        }
        return Task.FromResult(onlineUsers);
    }

    public static Task<List<string>> GetConnectionsForUser(string username)
    {
        List<string> connectionIds;
        if (OnlineUsers.TryGetValue(username, out var connections))
        {
            lock (connections)
            {
                connectionIds = connections.ToList();
            }
        }
        else
        {
            connectionIds = [];
        }
        return Task.FromResult(connectionIds);
    }
}