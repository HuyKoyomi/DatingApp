import { Component, inject, input, OnInit, output } from '@angular/core';
import { Member } from '../../_models/member';
import { DecimalPipe, NgClass, NgFor, NgIf, NgStyle } from '@angular/common';
import { FileUploader, FileUploadModule } from 'ng2-file-upload';
import { AccountService } from '../../_services/account.service';
import { environment } from '../../../environments/environment.development';
import { MembersService } from '../../_services/members.service';
import { ToastrService } from 'ngx-toastr';
import { Photo } from '../../_models/photo';

@Component({
  selector: 'app-photo-editor',
  standalone: true,
  imports: [NgIf, NgFor, NgStyle, NgClass, FileUploadModule, DecimalPipe],
  templateUrl: './photo-editor.component.html',
  styleUrl: './photo-editor.component.css',
})
export class PhotoEditorComponent implements OnInit {
  private accountService = inject(AccountService);
  private membersService = inject(MembersService);
  private toastr = inject(ToastrService);
  memberChange = output<Member>();
  member = input.required<Member>();

  uploader?: FileUploader; // Khởi tạo một FileUploader.
  hasBaseDropZoneOver = false; // Kiểm soát trạng thái khi file kéo vào vùng upload.
  baseUrl = environment.apiUrl;

  ngOnInit(): void {
    this.initializeUploader();
  }

  fileOverBase(e: any) {
    this.hasBaseDropZoneOver = e; // Khi người dùng kéo file vào vùng upload, trạng thái hasBaseDropZoneOver thay đổi
  }

  setMainPhoto(photo: Photo) {
    this.membersService.setMainPhoto(photo).subscribe({
      next: () => {
        const user = this.accountService.currentUser();
        if (user) {
          user.photoUrl = photo.url;
          this.accountService.setCurrentUser(user); // update lại store
        }
        const updateMember = { ...this.member() };
        updateMember.photoUrl = photo.url;
        updateMember.photos.map((p) => {
          if (p.isMain) p.isMain = false;
          if (p.id === photo.id) p.isMain = true;
        });
        this.memberChange.emit(updateMember); // update lai ảnh ở component cha
        this.toastr.success('Updated main photo successfully!');
      },
    });
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'users/add-photo',
      authToken: `Bearer ${this.accountService.currentUser()?.token}`,
      isHTML5: true, // Sử dụng HTML5 để upload.
      allowedFileType: ['image'], // Chỉ cho phép upload ảnh.
      removeAfterUpload: true, //Xóa file khỏi danh sách sau khi upload thành công.
      autoUpload: false, //Không tự động upload, user phải nhấn nút upload.
      maxFileSize: 10 * 1024 * 1024, //Giới hạn dung lượng 10MB.
    });

    // Ngăn trình duyệt gửi cookie hoặc thông tin credentials khi upload
    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    };

    // Xử lý khi upload thành công (onSuccessItem)
    this.uploader.onSuccessItem = (item, response, status, headers) => {
      const photo = JSON.parse(response); // Kiểm tra response có dữ liệu hay không => Chuyển response từ JSON thành Photo object.
      const updateMember = { ...this.member() }; // Tạo một bản sao của member và thêm ảnh mới vào.
      updateMember.photos.push(photo);
      this.memberChange.emit(updateMember); // Gửi dữ liệu cập nhật ra component cha
      // emit() được sử dụng với EventEmitter để gửi dữ liệu từ component con lên component cha.
    };
  }
}
