# **Section 11 summary** - Identity and role managenment

## I. Common info

1. **SignalR** là gì?

   - SignalR là một thư viện real-time của Microsoft giúp các ứng dụng web có thể giao tiếp hai chiều giữa client và server một cách dễ dàng

   - Các tính năng chính của SignalR

     - Real-time communication: Dữ liệu được cập nhật ngay lập tức giữa server và client.
     - Hỗ trợ nhiều giao thức truyền tải: WebSockets, Server-Sent Events (SSE), và Long Polling (tự động fallback nếu WebSockets không khả dụng).
     - Tích hợp dễ dàng với .NET: Hỗ trợ ASP.NET Core và .NET Framework.
     - Hỗ trợ group và user connection: Cho phép gửi tin nhắn đến một nhóm hoặc một user cụ thể.
     - Khả năng mở rộng: Kết hợp với Redis hoặc Azure SignalR để hỗ trợ nhiều kết nối.

   - Cách hoạt động của SignalR

     - Client kết nối với server thông qua WebSocket hoặc các phương thức fallback khác.
     - Server gửi thông báo đến client mà không cần client phải gửi request trước.
     - Client có thể gửi dữ liệu lên server và ngược lại theo cơ chế real-time.
     - Hỗ trợ nhiều client cùng lúc và quản lý kết nối thông qua Hub.

   - Khi nào nên sử dụng SignalR?
     - Ứng dụng chat real-time.
     - Bảng điều khiển live updates (Dashboard).
     - Ứng dụng thông báo (Notifications).
     - Multiplayer game online.
     - Theo dõi vị trí real-time (GPS tracking).
     - Ứng dụng truyền dữ liệu theo thời gian thực như giao dịch chứng khoán.

## II. BE

- 217. Adding a presence hub

  - Tạo folder **SignalR**
  - Tạo file Hub: để quản lý kết nối online/offline
  - Cấu hình trong ApplicationSvcExtensions
  - Cấu hình trong **Program.cs** đăng ký Hub SignalR trong ứng dụng ASP.NET Core.

- 218. Authenticating to SignalR

  - Xác thực Token từ Query String cho SignalR trong file _IdentitySvcExtensions_
  - Cấu hình Progaram.cs - AllowCredentials() → Cho phép cookies, tokens, hoặc thông tin xác thực khác được gửi giữa frontend và backend.

- 220. Adding a presence tracker

  - Thêm file **PresenceTracker.cs** // lưu danh sách các kết nối đang hoạt động
  - Thêm dịch vụ vào "ApplicationSvcExtensions" để sử dụng
  - gọi vào trong **PresenceHub.cs**

- 222. Creating a message hub

  - Tạo file **MessageHub.cs** để quản lý kết nối thời gian thực giữa client và server.
  - Thêm vào **Program.cs**

- 223. Adding the send message method to the hub

  - Tạo hàm SendMessage trong file **MessageHub.cs** để gửi dữ liệu

- 227. Tracking the message groups
  - Tạo file _Group.cs_, _Connection.cs_
  - Cấu hình trong _DataContext.cs_
  - Add function and gọi lệnh tạo bảng
    - **dotnet ef migrations add GroupsAdded**
- 228. Updating the message hub with group tracking

- 229. Dealing with UTC date formats

- 230. Notifying users when they receive a message

- 232. Optimizing the presence

- 233. Optimizing the messages

## III. FE

- 219. Client side SignalR

  - cài đặt thư viện **yarn add @microsoft/signalr**
  - thêm biến môi trường **hubsUrl: "https://localhost:5000/hubs/"**
  - Tạo file xử lý PresenceService **ng g s \_services/presence --skip-tests**
    - thềm hàm xử lý _createHubConnection_ và _stopHubConnect_
  - từ accountSvc => gọi 2 hàm trên

- 224. Adding the hub connection to the message service

  - tạo hàm **createHubConnection** trong _mesage.services.ts_

- 225. Refactoring the message components to use the hub

- 226. Sending messages via the hub

- 231. Subscribing to route parameter changes
