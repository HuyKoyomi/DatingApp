# **Section 12 summary** - Paging, sorting and filtering

1. BE
- Tạo PageList trong folder Helpers
- Tạo PaginationHeader.cs
- Tạo HttpExtension.cs
- Tạo USerParams.cs

Thêm file LogUserActivity để lưu log lịch sử hoạt động môi khi đăng nhập
Cấu hình scope trong ApplicationSvcExtensions.cs
Thêm vào cấu hình BaseApiController

[ServiceFilter(typeof(LogUserActivity))] là một attribute trong ASP.NET Core được sử dụng để áp dụng một Action Filter (trong trường hợp này là LogUserActivity) vào một controller hoặc action cụ thể.


2. FE





