# **Section 11 summary** - Identity and role managenment

1. Common info

- **_Microsoft.AspNetCore.Identity_** '

  - là một thư viện trong ASP.NET Core dùng để quản lý người dùng, vai trò (roles) và xác thực (authentication) trong ứng dụng web
  - Các thành phần chính trong Identity

    - User & Role Management

      - IdentityUser: Lớp đại diện cho người dùng (có sẵn các thuộc tính như UserName, Email, PasswordHash...)
      - IdentityRole: Đại diện cho một vai trò (Admin, User,...)
      - UserManager<TUser>: Dùng để thao tác với người dùng (tạo, cập nhật, xác thực...).
      - RoleManager<TRole>: Dùng để quản lý vai trò.

    - Authentication & Authorization
      - SignInManager<TUser>: Xác thực đăng nhập (cookie, JWT, 2FA).
      - Authorization: Kết hợp với Policy-based Authorization để kiểm soát quyền truy cập.

2. BE

- 197. Setting up the entities

  - sử dụng **_Microsoft.AspNetCore.Identity_** kế thừa bởi entity AppUser

  - Tạo file entity AppRole, AppUserRole và setup AppUser
  - Sửa lại các lỗi đỏ

- 198. Configuring the DbContext

  - thêm thư viện: **_Microsoft.AspNetCore.Identity.EntityFrameworkCore_**
  - sửa file _DataContext.cs_
  - Thiết lập quan hệ User (AppUser), Role (AppRole) và UserRole (AppUserRole) trong Entity Framework Core bằng Fluent API.

- 199. Configuring the startup class

  - cấu hình Identity

- Chỉnh sửa Username => UserName

  - dotnet ef migrations remove
  - dotnet ef migrations add FixUserRole
  - dotnet ef database update

- 200. Refactoring and adding a new migration

  - **dotnet build** : Biên dịch (Compile) mã nguồn => chỉ biên dịch mã nguồn của bạn mà không chạy ứng dụng
  - **dotnet run** : Chạy ứng dụng => Nếu chưa có build trước đó, nó sẽ tự động chạy **dotnet build**. => Nếu đã có build rồi, nó chỉ chạy ứng dụng mà không build lại.

- 201. Updating the seed method

  - _dotnet ef database drop_ : delet all db
  - run update lại data

- 202. Updating the account controller

- 203. Adding roles to the app

  - sửa file Seed.cs, Program.cs
  - drop table and run again

- 204. Adding the roles to the JWT token

  - theem roles trong tokenService

- 205. Adding policy based authorisation

  - cấu hình _IdentitySvcExtensions.cs_
  - Tạo file _AdminController_

- 206. Getting the users with roles
- 207. Editing user roles

3. FE

- 208. Adding an admin component

  - **"ng g c admin/admin-panel --skip-tests"**
  - cấu hình trong _router_

- 209. Adding an admin guard

  - **ng g guard \_guard/admin --skip-tests**
    - **ng**: Lệnh của Angular CLI.
    - **g**: (viết tắt của generate): Dùng để tạo một thành phần (component, service, guard, pipe, v.v.).
    - **guard**: Tạo một Angular Guard, dùng để bảo vệ route (chặn hoặc cho phép truy cập)

- 210. Adding a custom directive

  - **ng g d \_directives/has-role --dry-run**
    - **ng**: Lệnh Angular CLI.
    - **g**: Viết tắt của generate, tức là tạo một file hoặc một thành phần trong dự án Angular.
    - **d**: Viết tắt của directive, tức là tạo một directive.
    - **--dry-run**: Chỉ chạy mô phỏng mà không thực sự tạo file. Điều này giúp kiểm tra xem các file nào sẽ được tạo mà không làm thay đổi mã nguồn.
  - => thêm file _HasRoleDirective_
  - gọi và sử dụng với _NavComponent_
  - thêm thuộc tính _\*appHasRole="['Admin', 'Moderator']"_ vào thẻ link để nếu ko có quyền thì ko hiển thị

- 211. Adding the edit roles component

  - **ng g c admin/user-management --skip-tests**
  - **ng g c admin/photo-management --skip-tests**

  - **ng g s \_services/admin --skip-tests**

- 212. Setting up modals

  - cấu hình **app.config.ts** - thêm _ModalModule_
  - **ng g c modals/roles-modal --skip-tests**

- 213. Editing roles in the client