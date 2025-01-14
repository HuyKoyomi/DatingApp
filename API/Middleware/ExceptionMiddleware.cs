using System.Net;
using System.Text.Json;

namespace API;

/*
    + Middleware trong ASP.NET Core để xử lý ngoại lệ (exception) trong ứng dụng web API.
    + "bắt" tất cả các ngoại lệ không được xử lý, ghi log lỗi và trả về phản hồi (response) có định dạng JSON với thông tin lỗi
    1. Lớp ExceptionMiddleware
        + RequestDelegate next: Đại diện cho middleware tiếp theo trong pipeline
        + ILogger<ExceptionMiddleware> logger: Dùng để ghi log lỗi.
        + IHostEnvironment env: Cung cấp thông tin về môi trường chạy (Development, Production, v.v.).
*/
public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
{
    // phương thức quan trọng nhất của middleware, chịu trách nhiệm xử lý yêu cầu HTTP và ngoại lệ.
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context); // Thực thi middleware tiếp theo trong pipeline. Nếu không có lỗi, yêu cầu sẽ được xử lý bình thường
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message); // Ghi thông tin ngoại lệ vào hệ thống log.
            context.Response.ContentType = "application/json"; // Đặt kiểu nội dung (content type) là JSON.
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // Đặt mã trạng thái HTTP là 500 Internal Server Error.

            //Tạo đối tượng ApiException:
            var response = env.IsDevelopment() ?
            new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace)
            : new ApiException(context.Response.StatusCode, ex.Message, "Internal server error");

            // Cấu hình serializer
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            // Tuần tự hoá đối tượng ApiException thành chuỗi JSON.
            var json = JsonSerializer.Serialize(response, options);

            // Ghi chuỗi JSON vào phản hồi HTTP.
            await context.Response.WriteAsync(json);
        }
    }
}