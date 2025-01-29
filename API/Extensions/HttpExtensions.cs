using System.Text.Json;
using API.Helpers;

namespace API.Extensions;

// Đây là một extension method (phương thức mở rộng) được sử dụng để thêm header phân trang (Pagination) vào HTTP response trong API
public static class HttpExtensions
{

    // Đây là một phương thức mở rộng (extension method) cho HttpResponse.
    public static void AddPaginationHeader<T>(this HttpResponse response, PagedList<T> data)
    {
        // Tạo đối tượng PaginationHeader
        var paginationHeader = new PaginationHeader(data.CurrentPage, data.PageSize, data.TotalCount, data.TotalPages);

        // Cấu hình JSON cho header : PropertyNamingPolicy = JsonNamingPolicy.CamelCase => (ví dụ: currentPage thay vì CurrentPage).
        var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        // Chuyển đổi paginationHeader thành chuỗi JSON.
        response.Headers.Append("Pagination", JsonSerializer.Serialize(paginationHeader, jsonOptions));

        // Cho phép client đọc header Pagination
        response.Headers.Append("Access-Control-Expose-Headers", "Pagination");
    }
}