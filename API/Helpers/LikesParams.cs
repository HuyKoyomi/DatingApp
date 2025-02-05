namespace API.Helpers;

// sử dụng để lưu trữ và kiểm soát tham số phân trang (pagination parameters) khi truy vấn danh sách người dùng (User)
public class LikesParams : PagionationParams
{
    public string Predicate { get; set; } = "";
    public int UserId { get; set; }

}