namespace API.Helpers;

// sử dụng để lưu trữ và kiểm soát tham số phân trang (pagination parameters) khi truy vấn danh sách người dùng (User)
public class MessageParams : PagionationParams
{
    public string? UserName { get; set; }
    public string Container { get; set; } = "Unread";

}