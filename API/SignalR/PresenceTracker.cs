namespace API.SignalR;

public class PresenceTracker
{
    private static readonly Dictionary<string, List<string>> OnlineUsers = []; // lưu danh sách các kết nối đang hoạt động

    // Xử lý khi một user kết nối
    public Task<bool> UserConnected(string username, string connectionId)
    {
        var isOnline = false;
        lock (OnlineUsers) // Dùng lock (OnlineUsers) để đảm bảo luồng (thread-safe), tránh xung đột khi nhiều user kết nối cùng lúc.   
        {
            if (OnlineUsers.ContainsKey(username))
            {
                OnlineUsers[username].Add(connectionId); // Thêm connectionId vào danh sách
            }
            else
            {
                OnlineUsers.Add(username, [connectionId]); // Tạo một danh sách mới và thêm connectionId
                isOnline = true;
            }
        }
        return Task.FromResult(isOnline);
    }


    // Xử lý khi một user ngắt kết nối
    public Task<bool> UserDisconnected(string username, string connectionId)
    {
        var isOffline = false;
        lock (OnlineUsers)
        {
            if (!OnlineUsers.ContainsKey(username)) return Task.FromResult(isOffline) ;

            OnlineUsers[username].Remove(connectionId); // Xoá connectionId của user

            if (OnlineUsers[username].Count == 0)
            {
                OnlineUsers.Remove(username); // Nếu user không còn kết nối nào, xoá khỏi OnlineUsers
                isOffline = true;
            }
        }
        return Task.FromResult(isOffline);

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