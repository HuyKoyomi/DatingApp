# **Section 15 summary** - Adding the Messaging feature

1.  BE

    - Lession 178. Setting up the entities for messaging

      - Thêm Message.cs entity
      - Thêm liên kết với AppUser entity
      - Cấu hình quan hệ trong DataContext
      - dotnet ef migrations add MessageEntityAdded

    - Lession 179. Setting up message repository

      - Thêm file IMessageRepository
      - Thêm MessageDto
      - Thêm file MessageRepository
      - cấu hình scope

    - Lession 180. Setting up auto mapper profiles

    - Lession 181. Adding a mesage controller

    - Lession 182. Api GetMessagesForUSer

    - Lession 183. Getting the message thread for 2 users

      - Thêm hàm _GetMessageThread_ trong repository

        - **_ThenInclude _** được sử dụng để nạp dữ liệu liên quan cấp con (nested related data) trong quá trình truy vấn (eager loading)

        - khi bạn sử dụng **_Include _** để tải một thực thể liên quan, nếu thực thể đó lại có các thực thể con khác cần được tải theo, bạn sẽ dùng **_ThenInclude _** để chỉ định các mối quan hệ tiếp theo.

        - VD: Message có một thuộc tính điều hướng là Sender. Sender có một thuộc tính điều hướng là Photos.
          => Để tải đồng thời cả thông tin của Sender và các Photos của họ khi truy vấn Message

          - context.Messages
            .Include(x => x.Sender) // Nạp dữ liệu của Sender liên quan đến Message
            .ThenInclude(x => x.Photos) // Từ Sender, tiếp tục nạp dữ liệu của Photos

        - Lợi ích
          - giảm số lượng truy vấn đến cơ sở dữ liệu
          - Truy vấn rõ ràng và dễ bảo trì

      - sử dụng hàm trên trong Controller

- 193. Deleting messages on the API


2.  FE

- 185. Setting up the Angular app for messaging

  - sử dụng jsontots để tạo ra class trong client => tạo class Message.ts

  - sửa message.component.ts
    - _loadMessage()_, _getRouter()_

- 186. Adding the message thread in the client

  - thêm member-messages.component

- 188. Activating the message tab

- 189. Using query params

- 190. Using route resolvers

  - ng g r \_resolvers/member-detailed --skip-tests

- 191. Sending messages

  - thêm hàm _sendMessage()_ trong messageService
  - gọi hàm trên trong _member-message.component.ts_
  - sử dụng _FormModule_ trong Angular

- 192. Fixing the photo weirdness

- 194. Deleting messages on the client
