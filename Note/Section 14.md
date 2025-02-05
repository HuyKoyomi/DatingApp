# **Section 14 summary** - Adding the likes feature

1. BE
- Thêm một bảng phụ trong mối quan hệ many to many
    + Thêm model UserLike
    + Sửa lại AppUser.cs
    + Cập nhật trong DBContext
        - Thêm DbSet
        - sử dụng OnModelCreating để cấu hình quan hệ
        - chạy lênh *dotnet ef migrations add UserLikesAdded* để update db

2. FE





